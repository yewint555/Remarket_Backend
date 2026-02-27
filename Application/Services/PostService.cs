using Application.Dtos;
using Application.ServiceInterfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IApplicationDbcontext _dbContext;

    public PostService(IApplicationDbcontext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PostResponseDto> GetPostByIdAsync(Guid id)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.ItemStatus != ItemStatus.Removed);

        if (post == null) throw new KeyNotFoundException("Post not found");

        return PostMapping.MapEntityToResponse(post);
    }

    public async Task<PostResponseDto> CreatePostAsync(PostRequestDto dto, Guid userId)
    {
        var post = PostMapping.MapRequestToEntity(dto, userId);
        
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();

        return PostMapping.MapEntityToResponse(post);
    }

    public async Task<List<PostResponseDto>> GetUserPostsAsync(Guid userId)
    {
        return await _dbContext.Posts
            .Include(p => p.Images)
            .Where(p => p.UserId == userId && p.ItemStatus != ItemStatus.Removed)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => PostMapping.MapEntityToResponse(p))
            .ToListAsync();
    }

    public async Task<PostResponseDto> UpdatePostAsync(Guid id, PostRequestDto dto, Guid userId)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (post == null) throw new KeyNotFoundException("Post not found or unauthorized");

        PostMapping.MapUpdateDtoToEntity(new UpdatePostDto(
            dto.ItemName, dto.Price, dto.Description, dto.ItemCondition, dto.ItemStatus, dto.ImageUrls
        ), post);

        await _dbContext.SaveChangesAsync();
        return PostMapping.MapEntityToResponse(post);
    }

    public async Task<bool> DeletePostAsync(Guid id, Guid userId)
    {
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (post == null) return false;

        post.ItemStatus = ItemStatus.Removed; 
        
        await _dbContext.SaveChangesAsync();
        return true;
    }
}