using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IAuthService
{
    // Registration
    Task<string> RegisterV1Async(RegisterRequestDto dto);
    Task<string> RegisterV2Async(RegisterRequestDto dto);

    // Login and Token
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken);
    // Verification (OTP)
    Task<LoginResponseDto> VerifyOtpAsync(VerifyOtpRequestDto dto);
    Task<bool> ResendOtpAsync(ResendOtpRequestDto dto);

    // Password Reset
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);

    // Logout
    Task LogoutAsync(Guid userId);
}