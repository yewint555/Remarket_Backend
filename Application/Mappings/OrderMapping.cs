using Domain.Entities;
using Application.Dtos;
using System.Linq;

namespace Application.Mappings;

public static class OrderMapping
{
public static (Addresses Address, Orders Order) MapToEntities(CreateOrderRequestDto dto, Guid buyerId, decimal postPrice)
{
    var addressId = Guid.NewGuid();
    
    var addressEntity = new Addresses
    {
        Id = addressId,
        UserId = buyerId,
        HomeNumber = dto.HomeNumber,
        AptNumber = dto.AptNumber,
        Street = dto.Street,
        TownShip = dto.TownShip,
        City = dto.City,
        CreatedAt = DateTime.UtcNow
    };

    var orderEntity = new Orders
    {
        Id = Guid.NewGuid(),
        UserId = buyerId,
        PostId = dto.PostId,
        AddressId = addressId,
        PhoneNumber = dto.PhoneNumber,
        TotalAmount = postPrice,
        OrderDate = DateTime.UtcNow,
        OrderComfirmationStatus = Domain.Enum.OrderComfirmStatus.Pending,
        CreatedAt = DateTime.UtcNow
    };

    return (addressEntity, orderEntity);
}
    public static OrderResponseV1Dto MapToV1(Orders order)
{
    // ၁။ Address Formatting
    var addr = order.Address;
    string fullAddr = addr != null 
        ? $"{addr.HomeNumber}, {(string.IsNullOrEmpty(addr.AptNumber) ? "" : addr.AptNumber + ", ")}{addr.Street}, {addr.TownShip}, {addr.City}" 
        : "No Address Provided";

    var postImage = order.Post?.Images?
        .FirstOrDefault(img => img.PostId != null);

    var sellerProfileImage = order.Post?.User?.Images?
        .FirstOrDefault(img => img.PostId == null)?.ImgUrl;

    return new OrderResponseV1Dto(
        OrderId: order.Id,
        ImageId: postImage?.Id ?? Guid.Empty,
        ItemName: order.Post?.ItemName ?? "Unknown Item",
        Price: order.TotalAmount,
        CreatedAt: order.OrderDate,
        OderConfirmStatus: order.OrderComfirmationStatus.ToString(),
        BuyerName: order.User?.UserName ?? "Guest Buyer",
        PhoneNumber: order.PhoneNumber,
        FullAddress: fullAddr,
        PostImageUrl: postImage?.ImgUrl ?? "",
        SellerProfileImageUrl: sellerProfileImage ?? ""
    );
}
    public static OrderResponseV2Dto MapToV2(Orders order)
    {
        return new OrderResponseV2Dto(
            OrderId: order.Id,
            ItemName: order.Post?.ItemName ?? "Unknown Item",
            OderConfirmStatus: order.OrderComfirmationStatus.ToString(),
            PostImageUrl: order.Post?.Images?.FirstOrDefault()?.ImgUrl ?? "",
            CreatedAt: order.OrderDate
        );
    }
}