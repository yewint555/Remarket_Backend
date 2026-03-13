using Application.Dtos;
using Domain.Enum;

public interface IOrderService
{
    Task<Guid> PlaceOrderAsync(Guid buyerId, CreateOrderRequestDto dto);
    Task<OrderResponseV1Dto> GetOrderDetailsAsync(Guid orderId);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderComfirmStatus newStatus);

    Task<List<OrderResponseV1Dto>> GetSellerOrdersAsync(Guid userId, int pageNumber, int pageSize);
    Task<List<OrderResponseV2Dto>> GetBuyerOrdersAsync(Guid userId, int pageNumber, int pageSize);
}
