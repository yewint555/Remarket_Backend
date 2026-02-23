using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    public Task<ApiResponse<string>> FinalRegisterAsync(FinalRegisterRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> GoogleLoginAsync(GoogleLoginRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<bool>> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<bool>> ResendOtpAsync(ResendOtpRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<bool>> VerifyOtpAsync(VerityOtpRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> RegisterV1Async(RegisterV1RequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> RegisterV2Async(RegisterV2RequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        throw new NotImplementedException();
    }
}