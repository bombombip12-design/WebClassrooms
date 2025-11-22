# Hướng dẫn khắc phục lỗi Database

## Vấn đề
Lỗi: `Invalid column name 'PasswordHash'`, `Invalid column name 'SystemRoleId'`, etc.

Nguyên nhân: Database chưa có các bảng hoặc cấu trúc bảng không đúng với models.

## Giải pháp

### Cách 1: Sử dụng Entity Framework tự động tạo database (Khuyến nghị)

1. **Dừng ứng dụng** nếu đang chạy (nhấn Ctrl+C trong terminal hoặc dừng trong Visual Studio)

2. **Xóa database cũ** (nếu có):
   - Mở SQL Server Management Studio
   - Kết nối đến server `LAPTOP-RA8AK0H5`
   - Xóa database `DoAnChuyenNganh` nếu đã tồn tại

3. **Chạy lại ứng dụng**:
   ```bash
   dotnet run
   ```
   
   Ứng dụng sẽ tự động:
   - Tạo database `DoAnChuyenNganh` nếu chưa có
   - Tạo tất cả các bảng cần thiết
   - Thêm dữ liệu mẫu (SystemRoles: Admin, User)

### Cách 2: Chạy script SQL thủ công

1. **Mở SQL Server Management Studio**

2. **Kết nối đến server**: `LAPTOP-RA8AK0H5`

3. **Chạy script SQL** mà bạn đã cung cấp để tạo database và các bảng

4. **Đảm bảo** các bảng sau được tạo:
   - SystemRoles
   - Users
   - Classes
   - Enrollments
   - Assignments
   - AssignmentFiles
   - Submissions
   - SubmissionFiles
   - Comments
   - Announcements
   - AnnouncementFiles

5. **Kiểm tra** bảng `Users` có các cột:
   - Id (int, Primary Key, Identity)
   - FullName (nvarchar(255))
   - Email (nvarchar(255), Unique)
   - PasswordHash (nvarchar(500), nullable)
   - GoogleId (nvarchar(255), nullable)
   - AvatarUrl (nvarchar(500), nullable)
   - SystemRoleId (int, Foreign Key to SystemRoles)
   - CreatedAt (datetime)

### Cách 3: Sử dụng Migrations (Nâng cao)

1. **Dừng ứng dụng**

2. **Tạo migration**:
   ```bash
   dotnet ef migrations add InitialCreate
   ```

3. **Áp dụng migration**:
   ```bash
   dotnet ef database update
   ```

## Kiểm tra

Sau khi thực hiện một trong các cách trên:

1. **Mở SQL Server Management Studio**
2. **Kiểm tra database `DoAnChuyenNganh`** đã được tạo
3. **Kiểm tra bảng `Users`** có đầy đủ các cột như trên
4. **Kiểm tra bảng `SystemRoles`** có 2 dòng:
   - Id=1, RoleName='Admin'
   - Id=2, RoleName='User'

5. **Chạy lại ứng dụng** và thử đăng ký tài khoản mới

## Lưu ý

- Nếu vẫn gặp lỗi, hãy kiểm tra connection string trong `appsettings.json`
- Đảm bảo SQL Server đang chạy
- Đảm bảo có quyền tạo database và bảng trên SQL Server

