namespace Application.Dtos;

public record CreateOrderRequestDto
(
    Guid PostId,
    string PhoneNumber,
    string HomeNumber,
    string? AptNumber,
    string Street,
    string TownShip,
    string City
);

public record OrderResponseV1Dto
(
    Guid OrderId,
    Guid ImageId,
    string ItemName,
    decimal Price,
    DateTime CreatedAt,
    string OderConfirmStatus,
    string BuyerName,
    string PhoneNumber,
    string FullAddress,
    string PostImageUrl,
    string SellerProfileImageUrl
);

public record OrderResponseV2Dto
(
    Guid OrderId,
    string ItemName,    
    string OderConfirmStatus,
    string PostImageUrl,
    DateTime CreatedAt
);

