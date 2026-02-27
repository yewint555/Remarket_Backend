namespace Application.Dtos;

public record PostRequestDto(
    string ItemName,
    decimal Price,
    string Description,
    string ItemCondition,
    string ItemStatus,
    List<string> ImageUrls
);

public record PostResponseDto(
    Guid Id,
    string ItemName,
    decimal Price,
    string Description,
    string ItemCondition,
    string ItemStatus,
    List<string> ImageUrls,
    DateTime CreatedAt
);

public record UpdatePostDto(
    string ItemName,
    decimal Price,
    string Description,
    string ItemCondition,
    string ItemStatus,
    List<string> ImageUrls
);
