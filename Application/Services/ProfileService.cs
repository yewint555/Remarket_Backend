using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Application.Mappings;
using Domain.Entities;

namespace Application.Services;

public class ProfileService : IProfileService
{
    private readonly IApplicationDbcontext _dbContext;

    public ProfileService(IApplicationDbcontext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProfileResponseDto> GetProfileAsync(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.Images)
            .Include(u => u.Addresses)
            .Include(u => u.SocialMediaLinks)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) 
            throw new KeyNotFoundException("User not found");

            var LatestImage = user.Images?
                .OrderByDescending(img => img.Id)
                .FirstOrDefault();

        return ProfileMapping.MapToProfileResponseDto(
            user, 
            LatestImage, 
            user.Addresses?.FirstOrDefault()!,
            user.SocialMediaLinks?.FirstOrDefault()!
        );
    }

    public async Task<ProfileResponseDto> CreateProfileAsync(Guid userId, ProfileRequestDto dto)
    {
        var user = ProfileMapping.MapToUserEntity(dto);
        user.Id = userId;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return ProfileMapping.MapToProfileResponseDto(user, null, null, null);
    }

    public async Task<ProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto dto)
    {
        var user = await _dbContext.Users
            .Include(u => u.Images)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) 
            throw new KeyNotFoundException("User not found");

        ProfileMapping.MapUpdateToUserEntity(dto, user);

        if (!string.IsNullOrEmpty(dto.ImageUrl))
        {
            user.Images.Add(
                new Image
                {
                    UserId = userId,
                    ImgUrl = dto.ImageUrl,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        await _dbContext.SaveChangesAsync();
        
        return await GetProfileAsync(userId); 
    }

    public async Task<ProfileResponseDto> VerifyProfileAsync(Guid userId, VarifyProfileRequestDto dto)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) 
            throw new KeyNotFoundException("User not found");

        var address = ProfileMapping.MapToAdressEntity(userId, dto);
        _dbContext.Addresses.Add(address);

        var social = ProfileMapping.MapToSocialMediaLinkEntity(userId, dto);
        _dbContext.SocialMediaLinks.Add(social);

        user.TrustedVerified = true;

        await _dbContext.SaveChangesAsync();

        return await GetProfileAsync(userId);
    }
}