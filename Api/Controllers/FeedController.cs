using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class FeedsController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedsController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeeds([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _feedService.GetAllFeedsAsync(pageNumber, pageSize);
        
        return Ok(ApiResponse<PagedResponseDto<List<PostResponseDto>>>.Success(true, result, "Feeds retrieved successfully", 200));
    }
}