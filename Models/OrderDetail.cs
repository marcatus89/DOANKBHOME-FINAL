namespace DoAnTotNghiep.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // 🔹 Khóa ngoại đến Order
        public int OrderId { get; set; }

        // 🔹 Khóa ngoại đến Product
        public int ProductId { get; set; }

        // 🔹 Các thuộc tính hiển thị / giá trị
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // =====================================================
        // 🔸 Navigation properties (quan hệ với các bảng khác)
        // =====================================================

        // Mỗi OrderDetail thuộc về 1 Order
        public virtual Order? Order { get; set; }

        // Mỗi OrderDetail gắn với 1 Product (nếu cần)
        public virtual Product? Product { get; set; }
    }
}
