using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ApiWrappers;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderRequestDto dto)
    {
        var userId = GetUserId();
        var orderId = await _orderService.PlaceOrderAsync(userId, dto);
        
        var response = ApiResponse<Guid>.Success(true, orderId, "Order placed successfully", 200);
        return Ok(response);
    }

    [Authorize(Roles = "Buyer,Seller")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetails(Guid id)
    {
        var order = await _orderService.GetOrderDetailsAsync(id);
        
        var response = ApiResponse<OrderResponseV1Dto>.Success(true, order, "Order details retrieved successfully", 200);
        return Ok(response);
    }
    [Authorize(Roles = "Seller")]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] OrderComfirmStatus status)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, status);
        
        if (!result)
        {
            return NotFound(new { Message = "Order update failed" });
        }

        var response = ApiResponse<bool>.Success(true, result, $"Order status updated to {status}", 200);
        return Ok(response);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller-orders")]
    public async Task<IActionResult> GetSellerOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var orders = await _orderService.GetSellerOrdersAsync(userId, pageNumber, pageSize);
        
        var response = ApiResponse<List<OrderResponseV1Dto>>.Success(true, orders, "Seller orders retrieved successfully", 200);
        return Ok(response);
    }

    [Authorize(Roles = "Buyer")]
    [HttpGet("buyer-orders")]
    public async Task<IActionResult> GetBuyerOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var orders = await _orderService.GetBuyerOrdersAsync(userId, pageNumber, pageSize);
        
        var response = ApiResponse<List<OrderResponseV2Dto>>.Success(true, orders, "My order history retrieved successfully", 200);
        return Ok(response);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                          ?? User.FindFirstValue("uid");

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token.");

        return Guid.Parse(userIdClaim);
    }
}