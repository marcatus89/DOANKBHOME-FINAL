using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class StockLevel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required]
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        // --- THÊM LIÊN KẾT ĐẾN ĐƠN NHẬP HÀNG ---
        // Nullable vì hàng có thể được điều chỉnh từ Kiểm kê
        public int? PurchaseOrderItemId { get; set; } 
        [ForeignKey("PurchaseOrderItemId")]
        public virtual PurchaseOrderItem? PurchaseOrderItem { get; set; }
    }
}

