using Application.Dtos;
using Domain.Entities;

namespace Application.Mappings;

public class ProfileMapping
{
    public static void MapUpdateToUserEntity(UpdateProfileRequestDto dto, Users u)
    {
        u.UserName = dto.UserName;
        u.Phone = dto.PhoneNumber;
        u.Email = dto.Email;
    }

    public static Addresses MapToAdressEntity(Guid userId, VarifyProfileRequestDto dto)
    {
            var detailParts = dto.Address.Split(new[] { '/', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(p => p.Trim())
                                     .ToArray();

        return new Addresses
        {
            UserId = userId,
            City = "Yangon",

            HomeNumber = detailParts.Length > 0 ? detailParts[0] : "N/A",
            Street = detailParts.Length > 1 ? detailParts[1] : "N/A",
            TownShip = detailParts.Length > 2 ? detailParts[2] : "N/A",

            AptNumber = null 
    };
    }
    public static SocialMediaLinks MapToSocialMediaLinkEntity(Guid userId, VarifyProfileRequestDto dto)
    {
        return new SocialMediaLinks
        {
            UserId = userId,
            Links = dto.SocialMediaLink
        };
    }

    public static ProfileResponseDto MapToProfileResponseDto(Users u, Image? i, Addresses? a, SocialMediaLinks? s)
    {
        string? fullAddress = a != null 
            ? string.Join(", ", new[] { a.HomeNumber, a.AptNumber, a.Street, a.TownShip, a.City }
                .Where(str => !string.IsNullOrWhiteSpace(str))) 
            : null;

        return new ProfileResponseDto(
            u.Id,
            u.UserName,
            u.Phone,
            i?.ImgUrl,
            fullAddress,
            s?.Links,
            u.TrustedVerified
        );
    }
}