using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;

        [Required]
        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        // --- THÊM KHO ĐÍCH CHO ĐƠN NHẬP HÀNG ---
        [Required(ErrorMessage = "Vui lòng chọn kho nhận hàng.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn kho nhận hàng.")]
        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = "Đã đặt hàng"; 

        public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}

