using Application.Dtos;
using Application.Mappings;
using Application.ServiceInterfaces;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class FeedService : IFeedService
{
    private readonly IApplicationDbcontext _dbContext;

    public FeedService(IApplicationDbcontext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponseDto<List<PostResponseDto>>> GetAllFeedsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0) 
            throw new ArgumentException("Page number must be greater than 0.");
            
        if (pageSize <= 0 || pageSize > 100) 
            throw new ArgumentException("Page size must be between 1 and 100.");

        var query = _dbContext.Posts
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted && p.ItemStatus == ItemStatus.Active);

        var totalItems = await query.CountAsync();

        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => PostMapping.MapEntityToResponse(p))
            .ToListAsync();

        return new PagedResponseDto<List<PostResponseDto>>(totalItems, pageNumber, pageSize, posts);
    }
}