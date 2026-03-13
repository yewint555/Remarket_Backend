using System.Security.Claims;
using Application.Dtos;
using Application.Mappings;
using Application.ServiceInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbcontext _dbcontext;
    private readonly ITokenService _tokenService;
    private readonly IMemoryCache _cache;
    private readonly IEmailService _emailService;

    public AuthService(IApplicationDbcontext dbcontext, ITokenService tokenService, IMemoryCache cache, IEmailService emailService)
    {
        _dbcontext = dbcontext;
        _tokenService = tokenService;
        _cache = cache;
        _emailService = emailService;
    }

    public async Task<string> RegisterV1Async(RegisterRequestDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        if (await _dbcontext.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("Email already registered");

        var user = AuthMapping.MapRegisterV1ToUserEntity(dto);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.TrustedVerified = true;

        return await SaveToCacheAndSendOtp(dto.Email, user);
    }

    public async Task<string> RegisterV2Async(RegisterRequestDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        if (await _dbcontext.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("Email already registered");

        var user = AuthMapping.MapRegisterV2ToUserEntity(dto);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.TrustedVerified = true;

        return await SaveToCacheAndSendOtp(dto.Email, user);
    }

    public async Task<LoginResponseDto> VerifyOtpAsync(VerifyOtpRequestDto dto)
    {
        string cleanEmail = dto.Email.ToLower().Trim();
        string otpKey = $"OTP_{cleanEmail}";
        string userKey = $"PendingUser_{cleanEmail}";

        if (_cache.TryGetValue(otpKey, out string? storedOtp) && storedOtp == dto.Otp)
        {
            if (_cache.TryGetValue(userKey, out Users? pendingUser))
            {
                _dbcontext.Users.Add(pendingUser!);
                await _dbcontext.SaveChangesAsync();

                _cache.Remove(otpKey);
                _cache.Remove(userKey);

                return await GenerateAuthResponse(pendingUser!);
            }
            throw new ArgumentException("Registration session expired. Please register again.");
        }
        
        throw new ArgumentException("Invalid or expired OTP");
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        return await GenerateAuthResponse(user);
    }
    public async Task<bool> ResendOtpAsync(ResendOtpRequestDto dto)
    {
        if (_cache.TryGetValue($"PendingUser_{dto.Email}", out Users? user))
        {
            await SaveToCacheAndSendOtp(dto.Email, user!);
            return true;
        }
        throw new ArgumentException("Session expired. Please register again.");
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken)
{
    var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
    
    var userId = principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value 
                 ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
        throw new SecurityTokenException("Invalid token payload");

    var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

    if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
    {
        throw new SecurityTokenException("Invalid or expired refresh token. Please login again.");
    }

    return await GenerateAuthResponse(user);
}

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) throw new KeyNotFoundException("Email not found");

        var otp = new Random().Next(100000, 999999).ToString();

        string cleanEmail = dto.Email.ToLower().Trim();
        _cache.Set($"ResetOTP_{cleanEmail}", otp, TimeSpan.FromMinutes(5));
        
        await _emailService.SendOtpEmailAsync(dto.Email, otp);
        return true;
    }

    public async Task<bool> VerifyResetOtpAsync(VerifyOtpRequestDto dto)
    {
        string cleanEmail = dto.Email.ToLower().Trim();
        string cacheKey = $"ResetOTP_{cleanEmail}";
    
        if (_cache.TryGetValue(cacheKey, out string? storedOtp) && storedOtp == dto.Otp)
        {
            return true;
        }
    
        throw new ArgumentException("Invalid or expired OTP");
    }

public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
{
    if (dto.NewPassword != dto.ConfirmPassword)
        throw new ArgumentException("Passwords do not match");

    await VerifyResetOtpAsync(new VerifyOtpRequestDto(dto.Email, dto.Otp));

    var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user == null) throw new KeyNotFoundException("User not found");

    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
    
    user.RefreshToken = null; 
    user.RefreshTokenExpiry = null;

    _dbcontext.Users.Update(user);
    await _dbcontext.SaveChangesAsync();

    _cache.Remove($"ResetOTP_{dto.Email.ToLower().Trim()}");

    return true;
}

    private async Task<string> SaveToCacheAndSendOtp(string email, Users user)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        
        var cleanEmail = email.ToLower().Trim();
        _cache.Set($"PendingUser_{cleanEmail}", user, TimeSpan.FromMinutes(10));
        _cache.Set($"OTP_{cleanEmail}", otp, TimeSpan.FromMinutes(10));

        await _emailService.SendOtpEmailAsync(email, otp);
        return "OTP_SENT";
    }

    public async Task LogoutAsync(Guid userId)
{
    var user = await _dbcontext.Users.FindAsync(userId);
    if (user == null) throw new KeyNotFoundException("User not found.");

    user.RefreshToken = null;
    user.RefreshTokenExpiry = null;

    _dbcontext.Users.Update(user);
    await _dbcontext.SaveChangesAsync();
}

    private async Task<LoginResponseDto> GenerateAuthResponse(Users user)
{

    var accessToken = _tokenService.GenerateAccessToken(user);
    var refreshToken = _tokenService.GenerateRefreshToken();
    
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

    _dbcontext.Users.Update(user);
    await _dbcontext.SaveChangesAsync();

    return new LoginResponseDto(
        AccessToken: accessToken,
        RefreshToken: refreshToken,
        RefreshTokenExpiry: user.RefreshTokenExpiry ?? DateTime.UtcNow.AddDays(30),
        AccountType: user.AccountType.ToString()
    );
}
}