using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost("register-v1")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterV1Async(dto);
        return Ok(ApiResponse<string>.Success(true, result, "Registration successful", 200));
    }

    [HttpPost("register-v2")]
    public async Task<IActionResult> RegisterV2([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterV2Async(dto);
        return Ok(ApiResponse<string>.Success(true, result, "Registration successful", 200));
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto dto)
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
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);
        return Ok(ApiResponse<LoginResponseDto>.Success(true, result, "Refresh token successful", 200));
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequestDto dto)
    {
        var result = await _authService.ResendOtpAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "OTP resent successfully", 200));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "Password reset email sent", 200));
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = GetUserId();

        if (userId == null) 
            throw new UnauthorizedAccessException("Invalid user claim");
        await _authService.LogoutAsync(userId.Value);

        return Ok(ApiResponse<bool>.Success(true, true, "Logged out successfully", 200));
    }

    // Api/Controllers/AuthController.cs

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);
        return Ok(ApiResponse<bool>.Success(true, result, "Password reset successfully", 200));
    }
        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value 
                              ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var id))
            {
                return id;
            }
            return null;
        }
}