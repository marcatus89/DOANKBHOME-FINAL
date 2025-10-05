using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    // Model cho một Vị trí cụ thể trong kho (Kệ, Ô, Khu vực)
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }

        [Required(ErrorMessage = "Mã vị trí là bắt buộc.")]
        public string Code { get; set; } = string.Empty; // VD: A-01-B (Khu A - Kệ 01 - Tầng B)

        public string? Description { get; set; }
    }
}

