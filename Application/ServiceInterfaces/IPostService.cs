using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IPostService
{
    Task<PostResponseDto> GetPostByIdAsync(Guid id);
    Task<PostResponseDto> CreatePostAsync(PostRequestDto dto, Guid userId);
    // Task<PagedResponse<PostResponseDto>> GetAllPostsAsync(PaginationFilter filter);
    Task<List<PostResponseDto>> GetUserPostsAsync(Guid userId);
    Task<PostResponseDto> UpdatePostAsync(Guid id, PostRequestDto dto, Guid userId);
    Task<bool> DeletePostAsync(Guid id, Guid userId);
}