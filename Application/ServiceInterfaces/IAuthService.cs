using Application.ApiWrappers;
using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IAuthService
{
    // Registration
    Task<ApiResponse<string>> RegisterV1Async(RegisterV1RequestDto dto);
    Task<ApiResponse<string>> RegisterV2Async(RegisterV2RequestDto dto);
    Task<ApiResponse<string>> FinalRegisterAsync(FinalRegisterRequestDto dto);

    // Login and Token
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<ApiResponse<LoginResponseDto>> GoogleLoginAsync(GoogleLoginRequestDto dto);
    Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto);

    // Verification (OTP)
    Task<ApiResponse<bool>> VerifyOtpAsync(VerityOtpRequestDto dto);
    Task<ApiResponse<bool>> ResendOtpAsync(ResendOtpRequestDto dto);

    // Password Reset
    Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto dto);

    // Logout
    Task<ApiResponse<bool>> LogoutAsync();
}