using Microsoft.EntityFrameworkCore;
using DoAnTotNghiep.Models;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DoAnTotNghiep.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                // --- KHO & VỊ TRÍ ---
                if (!context.Warehouses.Any())
                {
                    logger.LogInformation("Seeding Warehouses and Locations...");
                    var mainWarehouse = new Warehouse { Name = "Kho Chính HCM", Address = "Quận 1, TP.HCM" };
                    var faultyWarehouse = new Warehouse { Name = "Kho Hàng Lỗi", Address = "Quận 9, TP.HCM" };
                    context.Warehouses.AddRange(mainWarehouse, faultyWarehouse);
                    context.SaveChanges();

                    context.Locations.AddRange(
                        new Location { WarehouseId = mainWarehouse.Id, Code = "A-01-01" },
                        new Location { WarehouseId = mainWarehouse.Id, Code = "A-01-02" },
                        new Location { WarehouseId = faultyWarehouse.Id, Code = "F-01" }
                    );
                    context.SaveChanges();
                    logger.LogInformation("Warehouses and Locations seeded successfully.");
                }

                // --- DANH MỤC ---
                if (!context.Categories.Any())
                {
                    logger.LogInformation("Seeding Categories...");
                    context.Categories.AddRange(
                        new Category { Name = "Bồn cầu" },
                        new Category { Name = "Vòi sen" },
                        new Category { Name = "Lavabo" },
                        new Category { Name = "Vòi Lavabo" }
                    );
                    context.SaveChanges();
                }

                // --- SẢN PHẨM ---
                if (!context.Products.Any())
                {
                    logger.LogInformation("Seeding Products...");
                    var bonCauCategory = context.Categories.First(c => c.Name == "Bồn cầu");
                    var voiSenCategory = context.Categories.First(c => c.Name == "Vòi sen");
                    var lavaboCategory = context.Categories.First(c => c.Name == "Lavabo");
                    var voiLavaboCategory = context.Categories.First(c => c.Name == "Vòi Lavabo");

                    context.Products.AddRange(
                        new Product
                        {
                            Name = "Bàn cầu điện tử MOEN - Walden HKSW1291C",
                            Price = 5000000M,
                            ImageUrl = "/images/product-1.jpg",
                            CategoryId = bonCauCategory.Id
                        },
                        new Product
                        {
                            Name = "Bộ sen tắm nóng lạnh SRN - 63332SRN",
                            Price = 2500000M,
                            ImageUrl = "/images/product-2.jpg",
                            CategoryId = voiSenCategory.Id
                        },
                        new Product
                        {
                            Name = "Lavabo đặt bàn Galassia DREAM 7300OC",
                            Price = 3000000M,
                            ImageUrl = "/images/product-3.jpg",
                            CategoryId = lavaboCategory.Id
                        },
                        new Product
                        {
                            Name = "Vòi lavabo Fiore Rubinetterie KUBE Chrome Black",
                            Price = 1800000M,
                            ImageUrl = "/images/product-4.jpg",
                            CategoryId = voiLavaboCategory.Id
                        }
                    );
                    context.SaveChanges();
                    logger.LogInformation("Products seeded successfully.");
                }

                // --- NHÀ CUNG CẤP ---
                if (!context.Suppliers.Any())
                {
                    logger.LogInformation("Seeding Suppliers...");
                    context.Suppliers.AddRange(
                        new Supplier { Name = "Công ty TNHH Thiết Bị Vệ Sinh Hòa Phát" },
                        new Supplier { Name = "Công ty TNHH Minh Phú" }
                    );
                    context.SaveChanges();
                    logger.LogInformation("Suppliers seeded successfully.");
                }

                // --- ĐƠN NHẬP HÀNG + CHI TIẾT ---
                if (!context.PurchaseOrders.Any())
                {
                    logger.LogInformation("Seeding PurchaseOrders...");
                    var supplier = context.Suppliers.First();
                    var warehouse = context.Warehouses.First();
                    var p1 = context.Products.First();
                    var p2 = context.Products.Skip(1).First();

                    var order = new PurchaseOrder
                    {
                        PurchaseOrderNumber = "PO-2025-001",
                        SupplierId = supplier.Id,
                        WarehouseId = warehouse.Id,
                        OrderDate = DateTime.Now.AddDays(-2),
                        Status = "Đã đặt hàng"
                    };

                    // ⚡ Lưu trước để có Id cho PurchaseOrder
                    context.PurchaseOrders.Add(order);
                    context.SaveChanges();

                    var item1 = new PurchaseOrderItem
                    {
                        PurchaseOrderId = order.Id, // ✅ Gắn rõ ràng quan hệ
                        ProductId = p1.Id,
                        Quantity = 10,
                        UnitPrice = 5000000M
                    };

                    var item2 = new PurchaseOrderItem
                    {
                        PurchaseOrderId = order.Id, // ✅ Gắn rõ ràng quan hệ
                        ProductId = p2.Id,
                        Quantity = 5,
                        UnitPrice = 2500000M
                    };

                    context.PurchaseOrderItems.AddRange(item1, item2);
                    context.SaveChanges();

                    logger.LogInformation("PurchaseOrders and Items seeded successfully.");
                }

                // --- TỒN KHO GẮN LIÊN VỚI PHIẾU NHẬP ---
                if (!context.StockLevels.Any())
                {
                    logger.LogInformation("Seeding StockLevels...");
                    var locA01 = context.Locations.First();
                    var p1 = context.Products.First();
                    var p2 = context.Products.Skip(1).First();

                    // ✅ Tìm đúng PurchaseOrderItem có quan hệ PO và Product
                    var poi1 = context.PurchaseOrderItems
                        .Include(poi => poi.PurchaseOrder)
                        .FirstOrDefault(poi => poi.ProductId == p1.Id);
                    var poi2 = context.PurchaseOrderItems
                        .Include(poi => poi.PurchaseOrder)
                        .FirstOrDefault(poi => poi.ProductId == p2.Id);

                    logger.LogInformation($"POI1 => {poi1?.Id}, PO#: {poi1?.PurchaseOrder?.PurchaseOrderNumber}");
                    logger.LogInformation($"POI2 => {poi2?.Id}, PO#: {poi2?.PurchaseOrder?.PurchaseOrderNumber}");

                    // ✅ Gắn PurchaseOrderItemId vào tồn kho để hiển thị đúng số phiếu nhập
                    context.StockLevels.AddRange(
                        new StockLevel
                        {
                            ProductId = p1.Id,
                            LocationId = locA01.Id,
                            Quantity = 10,
                            ReceivedDate = DateTime.Now.AddDays(-1),
                            PurchaseOrderItemId = poi1?.Id
                        },
                        new StockLevel
                        {
                            ProductId = p2.Id,
                            LocationId = locA01.Id,
                            Quantity = 5,
                            ReceivedDate = DateTime.Now.AddDays(-1),
                            PurchaseOrderItemId = poi2?.Id
                        }
                    );
                    context.SaveChanges();
                    logger.LogInformation("StockLevels seeded successfully with PurchaseOrder linkage.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while seeding database.");
            }
        }
    }
}
