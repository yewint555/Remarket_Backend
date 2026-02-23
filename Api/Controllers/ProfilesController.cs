using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> CreateProfile(Guid userId, [FromBody] ProfileRequestDto dto)
    {
        var result = await _profileService.CreateProfileAsync(userId, dto);
        return result.IsSuccess ? CreatedAtAction(nameof(GetProfile), new { userId = userId }, result) 
                               : StatusCode(result.StatusCode, result);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid userId, [FromBody] UpdateProfileRequestDto dto)
    {
        var result = await _profileService.UpdateProfileAsync(userId, dto);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPost("{userId:guid}/verify")]
    public async Task<IActionResult> VerifyProfile(Guid userId, [FromBody] VarifyProfileRequestDto dto)
    {
        var result = await _profileService.VerifyProfileAsync(userId, dto);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }
}