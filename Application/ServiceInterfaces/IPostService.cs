using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
    Task<PostResponseDto?> GetPostByIdAsync(Guid id);
    Task<PostResponseDto> CreatePostAsync(PostRequestDto dto, Guid userId);
    Task<PostResponseDto> UpdatePostAsync(Guid id, PostRequestDto dto, Guid userId);
    Task<bool> DeletePostAsync(Guid id, Guid userId);
}