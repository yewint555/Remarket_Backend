using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Application.Mappings;

namespace Application.Services;

public class ProfileService : IProfileService
{
    private readonly IApplicationDbcontext _dbContext;
    private readonly IImageService _imageService;

    public ProfileService(IApplicationDbcontext dbContext, IImageService imageService)
    {
        _dbContext = dbContext;
        _imageService = imageService;
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
    public async Task<ProfileResponseDto> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto dto)
{
    var user = await _dbContext.Users
        .Include(u => u.Images)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null) throw new KeyNotFoundException("User not found");

    ProfileMapping.MapUpdateToUserEntity(dto, user);

    if (!string.IsNullOrEmpty(dto.ImageUrl))
    {
        var savedImagePath = await _imageService.SaveImageAsync(dto.ImageUrl, "profiles");

        user.Images.Add(new Image
        {
            UserId = userId,
            ImgUrl = savedImagePath,
            ImgPath = savedImagePath, 
            CreatedAt = DateTime.UtcNow
        });
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