using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models
{
    // Model cho một Kho hàng hoặc Chi nhánh
    public class Warehouse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên kho là bắt buộc.")]
        public string Name { get; set; } = string.Empty; // VD: "Kho Chính HCM", "Kho Hàng Lỗi"

        public string? Address { get; set; }

        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}

