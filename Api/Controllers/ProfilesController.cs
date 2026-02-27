using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var result = await _profileService.GetProfileAsync(userId);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile retrieved successfully", 200));
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> CreateProfile(Guid userId, [FromBody] ProfileRequestDto dto)
    {
        var result = await _profileService.CreateProfileAsync(userId, dto);
        
        var response = ApiResponse<ProfileResponseDto>.Success(true, result, "Profile created successfully", 201);
        
        return CreatedAtAction(nameof(GetProfile), new { userId = userId }, response);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileRequestDto dto)
    {
        var result = await _profileService.UpdateProfileAsync(userId, dto);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile updated successfully", 200));
    }

    [HttpPost("{userId:guid}/verify")]
    public async Task<IActionResult> VerifyProfile(Guid userId, [FromBody] VarifyProfileRequestDto dto)
    {
        var result = await _profileService.VerifyProfileAsync(userId, dto);
        return Ok(ApiResponse<ProfileResponseDto>.Success(true, result, "Profile verified successfully", 200));
    }
}