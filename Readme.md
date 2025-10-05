Đồ án Tốt nghiệp: Website TMĐT & CRM "KBHOME"
Đây là đồ án cuối khóa, xây dựng một trang web thương mại điện tử hoàn chỉnh chuyên kinh doanh các thiết bị vệ sinh và phòng tắm cao cấp, tích hợp hệ thống CRM để quản lý quy trình nghiệp vụ nội bộ một cách chi tiết.

Công nghệ sử dụng
Backend & Frontend: Blazor Server (.NET)

Cơ sở dữ liệu: SQLite (cho môi trường phát triển)

ORM: Entity Framework Core

Xác thực & Phân quyền: ASP.NET Core Identity

Giao diện: Bootstrap 5, CSS tùy chỉnh

Thư viện phụ: NProgress.js (cho thanh tiến trình tải trang)

Các chức năng chính
1. Phía Khách hàng (E-commerce)
Giao diện: Chuyên nghiệp, hiện đại, responsive, lấy cảm hứng từ kbhome.vn.

Trang chủ: Slider ảnh động, trưng bày danh mục, sản phẩm nổi bật, tin tức, đối tác...

Duyệt sản phẩm: Xem danh sách sản phẩm (có phân trang), lọc theo danh mục, sắp xếp theo giá.

Tìm kiếm: Tìm kiếm sản phẩm theo tên.

Sản phẩm: Xem chi tiết sản phẩm.

Giỏ hàng & Thanh toán: Giỏ hàng động, quy trình thanh toán an toàn, tạo đơn hàng.

Người dùng: Đăng ký, đăng nhập, trang cá nhân xem lịch sử đơn hàng.

Trải nghiệm: Hiệu ứng tải trang, thông báo toast hiện đại.

2. Phía Quản trị (CRM)
Dashboard tổng quan: Cung cấp cái nhìn tổng quan về doanh thu, số đơn hàng.

Quản lý Sản phẩm: CRUD (Thêm, Sửa, Xóa) sản phẩm.

Quản lý Danh mục: CRUD danh mục.

Quản lý Người dùng:

Xem danh sách tất cả người dùng.

Tạo tài khoản nhân viên mới.

Phân quyền chi tiết cho từng tài khoản.

Phân hệ Mua hàng (Purchasing):

Quản lý Nhà cung cấp (CRUD).

Tạo và Quản lý Đơn nhập hàng (với mã phiếu tự động PN-xxxxx).

Phân hệ Kho (Warehouse):

Bảng điều khiển riêng cho Kho.

Quy trình Nhận hàng (cập nhật tồn kho tự động).

Quy trình Xuất hàng (chuẩn bị hàng cho đơn của khách).

Phân hệ Kinh doanh (Sales):

Bảng điều khiển riêng cho Sales, chỉ hiển thị các đơn hàng mới cần xác nhận.

Phân hệ Giao nhận (Logistics):

Bảng điều khiển riêng cho Giao nhận.

Tạo và quản lý Phiếu giao hàng, cập nhật mã vận đơn.

Hướng dẫn Chạy dự án
Mở dự án bằng Visual Studio Code.

Mở Terminal và chạy lệnh dotnet watch run.

Ứng dụng sẽ tự động tạo và khởi tạo dữ liệu cho database SQLite.

Tài khoản Demo
Admin:

Email: admin@kbhome.com

Password: Password123!

Nhân viên Sales:

Email: sales@kbhome.com

Password: Password123!

Nhân viên Mua hàng:

Email: purchasing@kbhome.com

Password: Password123!

Nhân viên Kho:

Email: warehouse@kbhome.com

Password: Password123!

Nhân viên Giao nhận:

Email: logistics@kbhome.com

Password: Password123!