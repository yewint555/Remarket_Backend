using System.Security.Claims;
using Application.Dtos;
using Application.Mappings;
using Application.ServiceInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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


    public async Task<string> RegisterAsync(RegisterRequestDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        if (await _dbcontext.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("Email already registered");

        var user = AuthMapping.MapRegisterToUserEntity(dto);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.TrustedVerified = true;

        return await SaveToCacheAndSendOtp(dto.Email, user);
    }

    public async Task<LoginResponseDto> VerifyOtpAsync(VerityOtpRequestDto dto)
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

                return GenerateAuthResponse(pendingUser!);
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

        return GenerateAuthResponse(user);
    }

    public async Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto dto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) throw new KeyNotFoundException("User not found. Please register first.");

        return GenerateAuthResponse(user);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(dto.RefreshToken);
        var email = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
            throw new UnauthorizedAccessException("Invalid token payload");

        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new UnauthorizedAccessException("User not found");

        return GenerateAuthResponse(user);
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

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) throw new KeyNotFoundException("Email not found");

        var otp = new Random().Next(100000, 999999).ToString();
        _cache.Set($"ResetOTP_{dto.Email}", otp, TimeSpan.FromMinutes(5));
        
        await _emailService.SendOtpEmailAsync(dto.Email, otp);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) throw new KeyNotFoundException("User not found");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _dbcontext.SaveChangesAsync();
        return true;
    }

    public Task<bool> LogoutAsync()
    {
        return Task.FromResult(true);
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

    private LoginResponseDto GenerateAuthResponse(Users user)
    {
        var token = _tokenService.GenerateAccessToken(user);
        return new LoginResponseDto(token, DateTime.UtcNow.AddMinutes(15));
    }
}