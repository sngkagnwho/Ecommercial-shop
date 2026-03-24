# MTKPM Admin Dashboard - Complete Guide

## ?? Overview

H? th?ng qu?n lý admin hoàn ch?nh cho d? án e-commerce MTKPM, ???c xây d?ng trên **ASP.NET Core Razor Pages** v?i **.NET 8**.

## ? Features (Tính N?ng)

### 1. **Dashboard** ??
- T?ng quan doanh s? bán hàng
- Bi?u ?? th?ng kê (Chart.js)
- ??n hàng g?n ?ây
- S?n ph?m h?t hàng
- Th?ng kê nhanh

### 2. **Qu?n lý S?n ph?m** ??
- Danh sách s?n ph?m v?i tìm ki?m
- T?o/S?a/Xóa s?n ph?m
- L?c theo danh m?c
- Qu?n lý hình ?nh
- Qu?n lý kho hàng

### 3. **Qu?n lý ??n hàng** ??
- Danh sách t?t c? ??n hàng
- Chi ti?t ??n hàng chi ti?t
- C?p nh?t tr?ng thái ??n hàng
- ?ánh d?u thanh toán
- Theo dõi v?n chuy?n

### 4. **Qu?n lý Ng??i dùng** ??
- Danh sách ng??i dùng
- Phân quy?n (Admin, Manager, User)
- Kích ho?t/Vô hi?u hóa tài kho?n
- Chi ti?t ng??i dùng
- Tìm ki?m theo tên/email

### 5. **Qu?n lý Danh m?c** ??
- T?o/S?a/Xóa danh m?c
- L?u d??i d?ng Grid cards
- Modal dialog cho thêm/s?a
- Hi?n th? s? s?n ph?m

### 6. **Qu?n lý Khuy?n mãi** ???
- T?o discount codes
- C?u hình lo?i (Percentage/Fixed)
- Qu?n lý th?i h?n
- Theo dõi l??t s? d?ng
- Tìm ki?m theo code

### 7. **Qu?n lý Thanh toán** ??
- L?ch s? giao d?ch
- Th?ng kê doanh thu
- Tr?ng thái thanh toán
- Hoàn ti?n
- L?c theo ph??ng th?c thanh toán

### 8. **Thông báo** ??
- G?i thông báo h? th?ng
- L?ch s? thông báo
- Phân lo?i thông báo
- Theo dõi ??c/ch?a ??c

### 9. **Cài ??t H? th?ng** ??
- C?u hình chung (Site, Currency, Timezone)
- C?u hình Email (SMTP)
- Cài ??t b?o m?t (Session, Lockout, 2FA)
- Ph??ng th?c thanh toán
- Cài ??t l?u tr? (Local, S3, Azure)

## ?? UI/UX Features

- **Responsive Design**: Ho?t ??ng t?t trên mobile, tablet, desktop
- **Modern Bootstrap 5**: Giao di?n hi?n ??i v?i Bootstrap 5
- **Font Awesome Icons**: 6.4.0 - H?n 2000 icons
- **Custom CSS**: Styling tùy ch?nh v?i color scheme xuyên su?t
- **Dark Sidebar**: Sidebar sidebar v?i navigation rõ ràng
- **Modal Dialogs**: Xác nh?n hành ??ng quan tr?ng
- **Toast Notifications**: Thông báo t?c th?i
- **Pagination**: Phân trang cho danh sách l?n

## ?? File Structure

```
frontend/mtkpm.Admin/
??? Views/
?   ??? Dashboard/
?   ?   ??? Index.cshtml          # Trang ch? admin
?   ??? Products/
?   ?   ??? Index.cshtml          # Danh sách s?n ph?m
?   ?   ??? Create.cshtml         # T?o s?n ph?m
?   ?   ??? Edit.cshtml           # S?a s?n ph?m
?   ?   ??? Details.cshtml        # Chi ti?t s?n ph?m
?   ??? Orders/
?   ?   ??? Index.cshtml          # Danh sách ??n hàng
?   ?   ??? Details.cshtml        # Chi ti?t ??n hàng
?   ??? Categories/
?   ?   ??? Index.cshtml          # Qu?n lý danh m?c
?   ??? Users/
?   ?   ??? Index.cshtml          # Qu?n lý ng??i dùng
?   ??? Discounts/
?   ?   ??? Index.cshtml          # Qu?n lý khuy?n mãi
?   ??? Payments/
?   ?   ??? Index.cshtml          # Qu?n lý thanh toán
?   ??? Notifications/
?   ?   ??? Index.cshtml          # Thông báo
?   ??? Settings/
?   ?   ??? Index.cshtml          # Cài ??t h? th?ng
?   ??? Shared/
?       ??? _Layout.cshtml        # Layout chính
??? wwwroot/
?   ??? css/
?   ?   ??? admin.css             # CSS tùy ch?nh
?   ??? js/
?       ??? admin.js              # JavaScript helper
??? Controllers/
?   ??? DashboardController.cs
?   ??? ProductsController.cs
?   ??? OrdersController.cs
?   ??? UsersController.cs
?   ??? CategoriesController.cs
?   ??? DiscountsController.cs
?   ??? PaymentsController.cs
?   ??? NotificationsController.cs
?   ??? SettingsController.cs
??? Models/
?   ??? Product/
?   ??? Order/
?   ??? User/
?   ??? Category/
?   ??? Discount/
?   ??? Payment/
?   ??? Notification/
?   ??? Dashboard/
??? Services/
?   ??? IProductService.cs
?   ??? IOrderService.cs
?   ??? IUserService.cs
?   ??? ...
??? Program.cs
```

## ?? Getting Started

### 1. Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (ho?c Visual Studio Code)
- Node.js (tùy ch?n, ?? frontend assets)

### 2. Installation
```bash
# Clone repository
git clone https://github.com/sngkagnwho/Ecommercial-shop.git

# Navigate to admin project
cd frontend/mtkpm.Admin

# Restore NuGet packages
dotnet restore

# Build project
dotnet build
```

### 3. Running
```bash
# Run development server
dotnet run

# Open browser
# http://localhost:5000/Dashboard
```

## ?? Configuration

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001",
    "RequestTimeoutSeconds": 30
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
}
```

## ?? Security Features

- JWT Authentication
- Role-based Authorization (Admin, Manager, User)
- CSRF Protection with AntiForgeryToken
- Session Management
- Two-Factor Authentication (tùy ch?n)
- HTTPS Enforcement
- Input Validation

## ?? Database Models

?ng d?ng t??ng tác v?i các entities sau:
- Product
- Order
- OrderItem
- User
- Category
- Discount
- Payment
- Notification

## ?? API Integration

Admin dashboard tích h?p v?i backend API:
- Products API
- Orders API
- Users API
- Categories API
- Discounts API
- Payments API
- Notifications API

## ?? Tính N?ng Chi Ti?t

### Products Management
```
- Hi?n th? danh sách s?n ph?m (paginates)
- Tìm ki?m theo tên, l?c theo danh m?c
- T?o s?n ph?m m?i v?i upload hình ?nh
- Ch?nh s?a thông tin s?n ph?m
- Xóa s?n ph?m (v?i xác nh?n)
- Hi?n th? chi ti?t s?n ph?m
- Qu?n lý kho hàng
- Hi?n th? tr?ng thái availability
```

### Orders Management
```
- Danh sách ??n hàng theo tr?ng thái
- Tìm ki?m theo s? ??n hàng
- Chi ti?t ??n hàng (items, t?ng ti?n)
- C?p nh?t tr?ng thái (Pending ? Delivered)
- Timeline tr?ng thái
- ?ánh d?u thanh toán
- H?y ??n hàng
```

### Users Management
```
- Danh sách ng??i dùng v?i roles
- Tìm ki?m theo tên/email
- L?c theo vai trò
- Kích ho?t/Vô hi?u hóa tài kho?n
- Chi ti?t ng??i dùng
- Phân quy?n
```

### Categories Management
```
- Hi?n th? danh m?c d??i d?ng cards
- T?o danh m?c m?i
- Ch?nh s?a thông tin danh m?c
- Xóa danh m?c
- Hi?n th? s? s?n ph?m trong danh m?c
```

### Discounts Management
```
- Danh sách khuy?n mãi
- Tìm ki?m theo discount code
- L?c theo tr?ng thái (Active, Expired, Inactive)
- Hi?n th? ki?u discount (Percentage, Fixed)
- Qu?n lý th?i h?n
- Theo dõi l??t s? d?ng
- T?o/S?a/Xóa discount
```

### Payments Management
```
- L?ch s? giao d?ch
- Th?ng kê doanh thu
- L?c theo tr?ng thái thanh toán
- L?c theo ph??ng th?c thanh toán
- X? lý hoàn ti?n
- Liên k?t t?i ??n hàng
```

### Notifications
```
- G?i thông báo cho ng??i dùng
- L?u l?ch s? thông báo
- Phân lo?i (System, Order, User, Alert)
- Theo dõi ?ã ??c/ch?a ??c
- Xóa thông báo
```

### Settings
```
- C?u hình Site (Tên, Mô t?, Currency, Timezone)
- C?u hình Email (SMTP, SSL/TLS)
- Cài ??t b?o m?t (Session, Lockout, 2FA)
- C?u hình thanh toán (Stripe, PayPal)
- C?u hình l?u tr? (Local, S3, Azure)
```

## ?? Best Practices Used

1. **Razor Pages Pattern**: Clean separation of concerns
2. **MVC Architecture**: Models, Views, Controllers/Handlers
3. **Dependency Injection**: All services injected via DI
4. **Repository Pattern**: For data access
5. **Async/Await**: For responsive UI
6. **Error Handling**: Proper exception handling
7. **Validation**: Client and server-side validation
8. **Responsive Design**: Mobile-first approach
9. **Accessibility**: ARIA labels, semantic HTML
10. **Performance**: Pagination, caching, optimization

## ?? Responsive Breakpoints

- **Mobile**: < 576px
- **Tablet**: 576px - 992px
- **Desktop**: > 992px

## ?? Workflow Examples

### T?o s?n ph?m
1. Click "Add New Product"
2. ?i?n thông tin (Name, Description, Price, Stock, Category)
3. Upload hình ?nh ho?c nh?p URL
4. Click "Create Product"
5. H? th?ng s? quay v? danh sách s?n ph?m

### C?p nh?t ??n hàng
1. Vào trang Orders
2. Click vào ??n hàng c?n c?p nh?t
3. Ch?n tr?ng thái m?i
4. Click "Update Status"
5. H? th?ng s? g?i thông báo cho khách hàng

### G?i Discount
1. Vào trang Discounts
2. Click "Add New Discount"
3. Nh?p thông tin (Code, Type, Value, Valid Period)
4. Click "Save"
5. Discount có th? ???c s? d?ng ngay

## ?? Troubleshooting

### 401 Unauthorized
- ??m b?o b?n ?ã login
- Ki?m tra JWT token h?t h?n
- Xóa cookies và login l?i

### 404 Not Found
- Ki?m tra URL route
- ??m b?o resource t?n t?i
- Ki?m tra permissions

### API Connection Error
- Ki?m tra backend API status
- Xác minh URL trong appsettings.json
- Ki?m tra CORS settings

## ?? Documentation

- [Bootstrap 5 Docs](https://getbootstrap.com/docs/5.3/)
- [Font Awesome Icons](https://fontawesome.com/icons)
- [ASP.NET Core Docs](https://docs.microsoft.com/en-us/aspnet/core/)
- [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)

## ?? License

MIT License - See LICENSE file

## ????? Contributing

Contributions are welcome! Please fork the repository and submit pull requests.

## ?? Support

For issues and questions, please open an issue on GitHub.

---

**Last Updated**: 2024
**Version**: 1.0.0
**Author**: MTKPM Development Team
