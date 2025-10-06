using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;

namespace DoAnTotNghiep.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn một danh mục.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        
        public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
        public bool IsPublished { get; set; } = false;

        // --- NÂNG CẤP LOGIC TÍNH TỒN KHO ---

        // Thuộc tính này tính tổng tồn kho vật lý (bao gồm cả hàng lỗi/mất)
        [NotMapped]
        public int TotalPhysicalStock => StockLevels?.Sum(sl => sl.Quantity) ?? 0;
        
        // Thuộc tính này chỉ tính tồn kho có thể bán (không bao gồm kho lỗi và vị trí hàng mất)
        [NotMapped]
        public int SellableStock => StockLevels?
            .Where(sl => sl.Location?.Warehouse?.Name != "Kho Hàng Lỗi" && sl.Location?.Code != "HANGMAT")
            .Sum(sl => sl.Quantity) ?? 0;
    }
}

