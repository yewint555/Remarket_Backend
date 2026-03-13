using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Controllers;

[Authorize (Roles = "Buyer,Seller")]
[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserIdFromToken();
        var result = await _profileService.GetProfileAsync(userId);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile retrieved successfully", 200));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto dto)
    {
        var userId = GetUserIdFromToken();
        var result = await _profileService.UpdateProfileAsync(userId, dto);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile updated successfully", 200));
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyProfile([FromBody] VarifyProfileRequestDto dto)
    {
        var userId = GetUserIdFromToken();
        var result = await _profileService.VerifyProfileAsync(userId, dto);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile verified successfully", 200));
    }
}