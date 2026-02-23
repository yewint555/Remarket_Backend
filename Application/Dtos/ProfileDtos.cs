namespace Application.Dtos;

public record ProfileRequestDto(
    
    string UserName,
    string Email,
    string PhoneNumber
);

public record VarifyProfileRequestDto(
    string City,
    string Address,
    string SocialMediaLink
);

public record UpdateProfileRequestDto(
    string UserName,
    string Email,
    string PhoneNumber,
    string? ImageUrl
);
public record ProfileResponseDto(
    Guid Id,
    string UserName,
    string PhoneNumber,
    string? ImageUrl,
    string? Address,
    string? SocialMediaLink,
    bool TrustedVerified
);