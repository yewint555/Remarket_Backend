using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IAuthService
{
    // Registration
    Task<string> RegisterAsync(RegisterRequestDto dto);

    // Login and Token
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> GoogleLoginAsync(GoogleLoginRequestDto dto);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);

    // Verification (OTP)
    Task<LoginResponseDto> VerifyOtpAsync(VerityOtpRequestDto dto);
    Task<bool> ResendOtpAsync(ResendOtpRequestDto dto);

    // Password Reset
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);

    // Logout
    Task<bool> LogoutAsync();
}