using Application.Dtos;

namespace Application.ServiceInterfaces;

public interface IProfileService
{
    Task<ProfileResponseDto> GetProfileAsync(Guid userId);
    
    Task<ProfileResponseDto> CreateProfileAsync(Guid userId, ProfileRequestDto dto);
    
    Task<ProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto dto);
    
    Task<ProfileResponseDto> VerifyProfileAsync(Guid userId, VarifyProfileRequestDto dto);
}