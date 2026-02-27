using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(ApiResponse<string>.Success(true, result, "Registration successful", 200));
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerityOtpRequestDto dto)
    {
        var result = await _authService.VerifyOtpAsync(dto);
        return Ok(ApiResponse<LoginResponseDto>.Success(true, result, "OTP verified successfully", 200));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(ApiResponse<LoginResponseDto>.Success(true, result, "Login successful", 200));
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto dto)
    {
        var result = await _authService.GoogleLoginAsync(dto);
        return Ok(ApiResponse<LoginResponseDto>.Success(true, result, "Google login successful", 200));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto);
        return Ok(ApiResponse<LoginResponseDto>.Success(true, result, "Token refreshed successfully", 200));
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequestDto dto)
    {
        var result = await _authService.ResendOtpAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "OTP resent successfully", 200));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "Password reset email sent", 200));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "Password reset successful", 200));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.LogoutAsync();
        return Ok(ApiResponse<bool>.Success(true, result, "Logged out successfully", 200));
    }
}