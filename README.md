# Google Classroom Clone - ASP.NET Core MVC

Ứng dụng web tương tự Google Classroom được xây dựng bằng ASP.NET Core MVC, C# và SQL Server.

## Tính năng

- **Quản lý người dùng**: Đăng ký, đăng nhập với xác thực cookie
- **Quản lý lớp học**: Tạo lớp học, tham gia lớp học bằng mã, xem danh sách lớp học
- **Quản lý bài tập**: Tạo, chỉnh sửa, xóa bài tập; đặt hạn nộp
- **Nộp bài**: Học sinh nộp bài tập, giáo viên chấm điểm và nhận xét
- **Thông báo**: Đăng thông báo trong lớp học
- **Phân quyền**: Phân biệt vai trò Teacher và Student

## Yêu cầu

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 hoặc VS Code

## Cài đặt

1. **Clone repository hoặc tải source code**

2. **Tạo database**:
   - Chạy script SQL trong SQL Server Management Studio để tạo database `DoAnChuyenNganh`

3. **Cấu hình connection string**:
   - Mở file `appsettings.json`
   - Cập nhật connection string phù hợp với SQL Server của bạn:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=DoAnChuyenNganh;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **Cài đặt packages**:
   ```bash
   dotnet restore
   ```

5. **Chạy ứng dụng**:
   ```bash
   dotnet run
   ```

6. **Truy cập ứng dụng**:
   - Mở trình duyệt và truy cập: `https://localhost:5001` hoặc `http://localhost:5000`

## Cấu trúc dự án

```
FinalASB/
├── Controllers/          # Các controller xử lý logic
│   ├── AccountController.cs
│   ├── HomeController.cs
│   ├── ClassesController.cs
│   ├── AssignmentsController.cs
│   ├── SubmissionsController.cs
│   └── AnnouncementsController.cs
├── Models/              # Các model entity
├── Views/               # Các view (Razor pages)
├── Data/                # DbContext và data access
├── ViewModels/          # View models
├── wwwroot/             # Static files (CSS, JS, images)
└── Program.cs           # Entry point và configuration
```

## Sử dụng

1. **Đăng ký tài khoản mới** hoặc **đăng nhập** nếu đã có tài khoản
2. **Tạo lớp học** hoặc **tham gia lớp học** bằng mã lớp học
3. Với vai trò **Teacher**: Tạo bài tập, đăng thông báo, chấm điểm
4. Với vai trò **Student**: Xem bài tập, nộp bài, xem điểm

## Lưu ý

- Mật khẩu được hash bằng BCrypt
- File đính kèm hiện tại hỗ trợ link Google Drive (có thể mở rộng để upload file trực tiếp)
- Session timeout: 30 phút
- Cookie authentication với remember me option

## Phát triển thêm

Có thể mở rộng thêm các tính năng:
- Upload file trực tiếp lên server
- Tích hợp Google Drive API
- Real-time notifications
- Comments trên bài tập
- Email notifications
- Mobile responsive improvements

