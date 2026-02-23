using Application.ApiWrappers;
using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IProfileService
{
    Task<ApiResponse<ProfileResponseDto>> GetProfileAsync(Guid userId);
    Task<ApiResponse<ProfileResponseDto>> CreateProfileAsync(Guid userId, ProfileRequestDto dto);
    Task<ApiResponse<ProfileResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto dto);
    Task<ApiResponse<ProfileResponseDto>> VerifyProfileAsync(Guid userId, VarifyProfileRequestDto dto);
}