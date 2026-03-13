using Application.Dtos;
using Application.ServiceInterfaces;
using Application.Mappings;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IApplicationDbcontext _dbContext;
    private readonly IImageService _imageService;   

    public PostService(IApplicationDbcontext dbContext, IImageService imageService)
    {
        _dbContext = dbContext;
        _imageService = imageService;
    }

    public async Task<PostResponseDto> GetPostByIdAsync(Guid id)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted && p.ItemStatus != ItemStatus.Removed);

        if (post == null) throw new KeyNotFoundException("Post not found");

        return PostMapping.MapEntityToResponse(post);
    }

    public async Task<PostResponseDto> CreatePostAsync(PostRequestDto dto, Guid userId)
{
    var post = PostMapping.MapRequestToEntity(dto, userId);
    
    if (dto.ImageUrls != null && dto.ImageUrls.Any())
    {
        foreach (var imageUrl in dto.ImageUrls)
        {
            var savedImagePath = await _imageService.SaveImageAsync(imageUrl, "posts");
            
            post.Images.Add(new Image
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ImgUrl = savedImagePath,
                ImgPath = savedImagePath,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            });
        }
    }

    _dbContext.Posts.Add(post);
    await _dbContext.SaveChangesAsync(); 

    return PostMapping.MapEntityToResponse(post);
}

    public async Task<List<PostResponseDto>> GetUserPostsAsync(Guid userId)
    {
        var posts = await _dbContext.Posts
            .Include(p => p.Images)
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

            if (posts == null )throw new KeyNotFoundException("No posts found for this user");

        return posts.Select(PostMapping.MapEntityToResponse).ToList();

    }

    public async Task<PostResponseDto> UpdatePostAsync(Guid id, PostRequestDto dto, Guid userId)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId && !p.IsDeleted);
    
        if (post == null) throw new KeyNotFoundException("Post not found or unauthorized");
    
        var updateDto = new UpdatePostDto(
            dto.ItemName,
            dto.Price,
            dto.Description,
            dto.ItemCondition,
            dto.ItemStatus,
            dto.ImageUrls
        );
        PostMapping.MapUpdateDtoToEntity(updateDto, post);
    
        if (dto.ImageUrls != null && dto.ImageUrls.Any())
        {
            foreach (var imageUrl in dto.ImageUrls)
            {
                if (imageUrl.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    var savedImagePath = await _imageService.SaveImageAsync(imageUrl, "posts");
                    
                    var newImage = new Image
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        PostId = post.Id,
                        ImgUrl = savedImagePath,
                        ImgPath = savedImagePath,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    
                    _dbContext.Images.Add(newImage);
                }
            }
        }
        await _dbContext.SaveChangesAsync();
    
        return PostMapping.MapEntityToResponse(post);
    }

    public async Task<bool> DeletePostAsync(Guid id, Guid userId)
    {
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (post == null) return false;

        post.ItemStatus = ItemStatus.Removed; 

        post.IsDeleted = true;
        
        await _dbContext.SaveChangesAsync();
        return true;
    }
}