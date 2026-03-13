using Application.Dtos;
using Application.Mappings;
using Application.ServiceInterfaces;
using Domain.Enum;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbcontext _dbContext;

    public OrderService(IApplicationDbcontext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> PlaceOrderAsync(Guid buyerId, CreateOrderRequestDto dto)
    {
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == dto.PostId && p.ItemStatus == ItemStatus.Active);

        if (post == null) 
            throw new Exception("The item is no longer available for purchase.");

        var (address, order) = OrderMapping.MapToEntities(dto, buyerId, post.Price);

        _dbContext.Addresses.Add(address);
        _dbContext.Orders.Add(order);

        post.ItemStatus = ItemStatus.Sold;

        await _dbContext.SaveChangesAsync();
        return order.Id;
    }

    public async Task<OrderResponseV1Dto> GetOrderDetailsAsync(Guid orderId)
    {
        var order = await GetOrderBaseQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) 
            throw new KeyNotFoundException("Order not found.");

        return OrderMapping.MapToV1(order);
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderComfirmStatus newStatus)
    {
        var order = await _dbContext.Orders
            .Include(o => o.Post)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) 
            throw new KeyNotFoundException("Cannot update status. Order not found.");

        order.OrderComfirmationStatus = newStatus;

        if (newStatus == OrderComfirmStatus.Cancelled && order.Post != null)
        {
            order.Post.ItemStatus = ItemStatus.Active;
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }
    public async Task<List<OrderResponseV1Dto>> GetSellerOrdersAsync(Guid userId, int pageNumber, int pageSize)
    {
        var orders = await GetOrderBaseQuery()
            .Where(o => o.Post.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return orders.Select(OrderMapping.MapToV1).ToList();
    }

    public async Task<List<OrderResponseV2Dto>> GetBuyerOrdersAsync(Guid userId, int pageNumber, int pageSize)
    {
        var orders = await _dbContext.Orders
            .Include(o => o.Post)
                .ThenInclude(p => p.Images)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return orders.Select(OrderMapping.MapToV2).ToList();
    }

    private IQueryable<Orders> GetOrderBaseQuery()
    {
        return _dbContext.Orders
            .Include(o => o.Address)
            .Include(o => o.User)
            .Include(o => o.Post)
                .ThenInclude(p => p.Images)
            .Include(o => o.Post)
                .ThenInclude(p => p.User)
                    .ThenInclude(u => u.Images);
    }
}