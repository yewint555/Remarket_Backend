using Application.ApiWrappers;
using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Application.Mappings;
using System.Security.Cryptography.X509Certificates;

namespace Application.Services;

public class ProfileService : IProfileService
    {
        private readonly IApplicationDbcontext _dbContext;

        public ProfileService(IApplicationDbcontext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<ProfileResponseDto>> GetProfileAsync(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.Images)
            .Include(u => u.Addresses)
            .Include(u => u.SocialMediaLinks)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return ApiResponse<ProfileResponseDto>.Failure("User not found", 404);
        }

        var responseDto = ProfileMapping.MapToProfileResponseDto(
            user, 
            user.Images?.FirstOrDefault(), 
            user.Addresses?.FirstOrDefault()!,
            user.SocialMediaLinks?.FirstOrDefault()!
        );

        return ApiResponse<ProfileResponseDto>.Success(true, responseDto, "Profile retrieved successfully", 200);

    }
    
        public async Task<ApiResponse<ProfileResponseDto>> CreateProfileAsync(Guid userId, ProfileRequestDto dto)
    {
        var user = ProfileMapping.MapToUserEntity(dto);
        user.Id = userId;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var response = ProfileMapping.MapToProfileResponseDto(user, null, null, null);
        return ApiResponse<ProfileResponseDto>.Success(true, response, "Profile created successfully", 201);
    }

    public async Task<ApiResponse<ProfileResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto dto)
    {
        var user = await _dbContext.Users
            .Include(u => u.Images)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return ApiResponse<ProfileResponseDto>.Failure("User not found", 404);

        ProfileMapping.MapUpdateToUserEntity(dto, user);

        if (!string.IsNullOrEmpty(dto.ImageUrl))
        {
            var existingImage = user.Images.FirstOrDefault();
            if (existingImage != null)
            {
                ProfileMapping.MapUpdateToImageEntity(dto.ImageUrl, existingImage, userId);
            }
            else
            {
                user.Images.Add(new Image { UserId = userId, ImgUrl = dto.ImageUrl });
            }
        }

        await _dbContext.SaveChangesAsync();
        
        return await GetProfileAsync(userId); 
    }

    public async Task<ApiResponse<ProfileResponseDto>> VerifyProfileAsync(Guid userId, VarifyProfileRequestDto dto)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return ApiResponse<ProfileResponseDto>.Failure("User not found", 404);

        var address = ProfileMapping.MapToAdressEntity(userId, dto);
        _dbContext.Addresses.Add(address);

        var social = ProfileMapping.MapToSocialMediaLinkEntity(userId, dto);
        _dbContext.SocialMediaLinks.Add(social);

        user.TrustedVerified = true;

        await _dbContext.SaveChangesAsync();

        return await GetProfileAsync(userId);
    }

}