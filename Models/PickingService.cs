using DoAnTotNghiep.Data;
using DoAnTotNghiep.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnTotNghiep.Services
{
    public class PickingService
    {
        private readonly ApplicationDbContext _dbContext;

        public PickingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Lớp ViewModel để chứa gợi ý cho một sản phẩm
        public class PickingSuggestion
        {
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
            public int RequiredQuantity { get; set; }
            public List<PickingDetail> SuggestedPicks { get; set; } = new List<PickingDetail>();
        }

        public class PickingDetail
        {
            public int StockLevelId { get; set; }
            public int LocationId { get; set; }
            public string? LocationCode { get; set; }
            public int AvailableQuantity { get; set; }
            public int QuantityToPick { get; set; }
        }

        // Hàm chính để tạo gợi ý
        public async Task<List<PickingSuggestion>> GetPickingSuggestionsAsync(int orderId)
        {
            var suggestions = new List<PickingSuggestion>();
            var order = await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return suggestions;

            foreach (var item in order.OrderDetails)
            {
                var suggestion = new PickingSuggestion
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    RequiredQuantity = item.Quantity
                };

                int quantityNeeded = item.Quantity;

                // Lấy tất cả các lô hàng trong kho chính, sắp xếp theo FIFO
                var stockLevels = await _dbContext.StockLevels
                    .Include(sl => sl.Location)
                    .ThenInclude(l => l.Warehouse)
                    .Where(sl => sl.ProductId == item.ProductId 
                                 && sl.Location.Warehouse.Name.Contains("Kho Chính")
                                 && sl.Quantity > 0)
                    .OrderBy(sl => sl.ReceivedDate) // QUY TẮC FIFO
                    .ToListAsync();

                // Tạo gợi ý từ các lô hàng tìm thấy
                foreach (var stock in stockLevels)
                {
                    if (quantityNeeded <= 0) break;

                    int quantityToPick = System.Math.Min(quantityNeeded, stock.Quantity);

                    suggestion.SuggestedPicks.Add(new PickingDetail
                    {
                        StockLevelId = stock.Id,
                        LocationId = stock.LocationId,
                        LocationCode = stock.Location.Code,
                        AvailableQuantity = stock.Quantity,
                        QuantityToPick = quantityToPick
                    });

                    quantityNeeded -= quantityToPick;
                }
                suggestions.Add(suggestion);
            }
            return suggestions;
        }
    }
}

