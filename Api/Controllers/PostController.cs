using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }

    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostRequestDto dto)
    {
        var userId = GetUserIdFromToken();
        var result = await _postService.CreatePostAsync(dto, userId);
        return Ok(ApiResponse<PostResponseDto>.Success(true, result, "Post created successfully", 201));
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller-posts")]
    public async Task<IActionResult> GetMyPosts()
    {
        var userId = GetUserIdFromToken();
        var result = await _postService.GetUserPostsAsync(userId);
        return Ok(ApiResponse<List<PostResponseDto>>.Success(true, result, "User posts retrieved successfully", 202));
    }

    [Authorize(Roles = "Buyer,Seller")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPost(Guid id)
    {
        var result = await _postService.GetPostByIdAsync(id);
        return Ok(ApiResponse<PostResponseDto>.Success(true, result, "Post retrieved successfully", 200));
    }

    [Authorize(Roles = "Seller")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] PostRequestDto dto)
    {
        var userId = GetUserIdFromToken();
        var result = await _postService.UpdatePostAsync(id, dto, userId);
        return Ok(ApiResponse<PostResponseDto>.Success(true, result, "Post updated successfully", 200));
    }

    [Authorize(Roles = "Seller")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var userId = GetUserIdFromToken();
        var result = await _postService.DeletePostAsync(id, userId);
        
        if (!result)
        {
            return NotFound(ApiResponse<bool>.Success(false, false, "Post not found or unauthorized", 404));
        }

        return Ok(ApiResponse<bool>.Success(true, true, "Post deleted successfully", 200));
    }
}