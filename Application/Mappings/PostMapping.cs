using Application.Dtos;
using Domain.Entities;
using Domain.Enum;

namespace Application.Mappings;

public class PostMapping
{
    public static Posts MapRequestToEntity(PostRequestDto dto, Guid userId)
    {
        return new Posts
        {
            Id = Guid.NewGuid(),
            ItemName = dto.ItemName,
            Price = dto.Price,
            Description = dto.Description,
            UserId = userId,
            
            ItemCondition = Enum.TryParse<ItemCondition>(dto.ItemCondition, true, out var condition) 
                ? condition : ItemCondition.New,
            
            ItemStatus = Enum.TryParse<ItemStatus>(dto.ItemStatus, true, out var status) 
                ? status : ItemStatus.Active,
    
            Images = dto.ImageUrls.Select(url => new Image 
            { 
                Id = Guid.NewGuid(),
                ImgUrl = url 
            }).ToList(),
    
        };
    }

public static PostResponseDto MapEntityToResponse(Posts p)
{
    return new PostResponseDto(
        p.Id,
        p.ItemName,
        p.Price,
        p.Description,
        p.ItemCondition.ToString(),
        p.ItemStatus.ToString(),
        p.Images.Select(img => img.ImgUrl!).ToList(),
        DateTime.UtcNow
    );
}

public static void MapUpdateDtoToEntity(UpdatePostDto dto, Posts existingPost)
{
    existingPost.ItemName = dto.ItemName;
    existingPost.Price = dto.Price;
    existingPost.Description = dto.Description;
    
    if (Enum.TryParse(dto.ItemCondition, out ItemCondition condition))
        existingPost.ItemCondition = condition;
        
    if (Enum.TryParse(dto.ItemStatus, out ItemStatus status))
        existingPost.ItemStatus = status;

    existingPost.Images = dto.ImageUrls.Select(url => new Image { ImgUrl = url }).ToList();
    
    existingPost.UpdatedAt = DateTime.UtcNow;
}
}