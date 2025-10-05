using System;
using System.Linq;
using System.Threading.Tasks;
using DoAnTotNghiep.Data;
using DoAnTotNghiep.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DoAnTotNghiep.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CartService _cartService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext dbContext, CartService cartService, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<(bool Success, string ErrorMessage)> PlaceOrderAsync(Order order, ClaimsPrincipal user)
        {
            var cartItems = _cartService.Items;
            if (!cartItems.Any()) return (false, "Giỏ hàng của bạn đang trống.");

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            order.OrderDate = DateTime.Now;
            order.TotalAmount = _cartService.Total;
            order.Status = "Chờ xác nhận";
            order.UserId = userId;

            // --- LOGIC KIỂM TRA TỒN KHO KHẢ DỤNG (KHÔNG TRỪ KHO) ---
            foreach (var item in cartItems)
            {
                // 1. Lấy tổng tồn kho vật lý
                var physicalStock = await _dbContext.StockLevels
                    .Where(sl => sl.ProductId == item.ProductId)
                    .SumAsync(sl => sl.Quantity);

                // 2. Lấy tổng số lượng đã được "tạm giữ" cho các đơn hàng khác
                var allocatedStock = await _dbContext.OrderDetails
                    .Include(od => od.Order)
                    .Where(od => od.ProductId == item.ProductId && 
                                 (od.Order.Status == "Chờ xác nhận" || 
                                  od.Order.Status == "Chờ thanh toán" || 
                                  od.Order.Status == "Đã thanh toán"))
                    .SumAsync(od => od.Quantity);
                
                // 3. Tồn kho có thể bán = Tồn kho vật lý - Tồn kho đã tạm giữ
                var availableStock = physicalStock - allocatedStock;

                if (availableStock < item.Quantity)
                {
                    return (false, $"Xin lỗi, sản phẩm '{item.ProductName}' không đủ số lượng để đặt (chỉ còn {availableStock} sản phẩm khả dụng).");
                }

                var orderDetail = new OrderDetail 
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName ?? string.Empty,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                order.OrderDetails.Add(orderDetail);
            }

            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
                _cartService.ClearCart();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lưu đơn hàng.");
                return (false, "Đã có lỗi xảy ra khi lưu đơn hàng. Vui lòng thử lại.");
            }
        }
    }
}

