# 🛒 Ecommercial Shop

A modern e-commerce platform built with .NET 8/.NET 10 and Razor Pages, providing a complete shopping experience with user management, product catalog, cart management, and order processing.

## ✨ Features

### Core Functionality
- 🔐 **Authentication & Authorization** - User registration and login system
- 📦 **Product Management** - Browse, search, and filter products by categories
- 🛍️ **Shopping Cart** - Add/remove items, manage quantities, and checkout
- ❤️ **Favorites** - Save favorite products for later
- 💳 **Payment Processing** - Secure payment integration
- 📋 **Order Management** - Track orders and view order history
- 📍 **User Addresses** - Manage multiple delivery addresses
- 💰 **Pricing & Discounts** - Apply discount codes and promotional pricing
- 🔔 **Notifications** - Real-time updates on orders and promotions

### Technical Features
- Exception handling middleware for robust error management
- Request/response logging for monitoring
- Input validation behavior
- RESTful API architecture
- Responsive web interface

## 🏗️ Project Structure

```
Ecommercial-shop/
├── backend/                          # Backend API services
│   ├── mtkpm/                        # Main API project (.NET 8)
│   │   ├── Controllers/              # API endpoints
│   │   │   ├── AuthController.cs
│   │   │   ├── ProductsController.cs
│   │   │   ├── CartController.cs
│   │   │   ├── OrdersController.cs
│   │   │   ├── PaymentController.cs
│   │   │   ├── DiscountController.cs
│   │   │   └── ...
│   │   ├── Middleware/               # Custom middlewares
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   ├── RequestResponseLoggingMiddleware.cs
│   │   │   └── ValidationBehavior.cs
│   │   └── Program.cs                # Application configuration
│   │
│   ├── mtkpm.Application/            # Business logic layer (.NET 8)
│   ├── mtkpm.Domain/                 # Domain models (.NET 8)
│   └── mtkpm.Infrastructure/         # Data access & external services (.NET 8)
│
└── frontend/                         # Frontend UI applications
    ├── mtkpm.UI/                     # Main customer UI (.NET 8)
    │   ├── Controllers/              # Page controllers
    │   ├── Views/                    # Razor Pages & layouts
    │   └── Models/                   # View models
    │
    └── mtkpm.Admin/                  # Admin dashboard (.NET 10)
```

## 🔧 Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | ASP.NET Core 8, .NET 10 |
| **Frontend** | Razor Pages, ASP.NET Core MVC |
| **Architecture** | Layered architecture (Domain, Application, Infrastructure) |
| **API** | RESTful API |
| **Pattern** | Clean Architecture |

## 📋 Prerequisites

- .NET 8 SDK or higher
- .NET 10 SDK (for Admin dashboard)
- Visual Studio 2022 or VS Code
- SQL Server or compatible database

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/sngkagnwho/Ecommercial-shop.git
cd Ecommercial-shop
```

### 2. Setup Backend
```bash
cd backend/mtkpm
dotnet restore
dotnet build
```

### 3. Setup Frontend (Customer UI)
```bash
cd frontend/mtkpm.UI
dotnet restore
dotnet build
```

### 4. Setup Admin Dashboard
```bash
cd frontend/mtkpm.Admin
dotnet restore
dotnet build
```

### 5. Run the Application

**Backend API:**
```bash
cd backend/mtkpm
dotnet run
```
The API will be available at `https://localhost:5001`

**Customer Frontend:**
```bash
cd frontend/mtkpm.UI
dotnet run
```
Access at `https://localhost:3000`

**Admin Dashboard:**
```bash
cd frontend/mtkpm.Admin
dotnet run
```
Access at `https://localhost:3001`

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product details
- `GET /api/categories` - Get product categories
- `GET /api/products/category/{categoryId}` - Get products by category

### Shopping Cart
- `POST /api/cart/add` - Add item to cart
- `DELETE /api/cart/remove/{itemId}` - Remove item from cart
- `GET /api/cart` - Get cart contents
- `PUT /api/cart/update` - Update cart

### Orders
- `POST /api/orders` - Create new order
- `GET /api/orders` - Get user orders
- `GET /api/orders/{id}` - Get order details

### Payments
- `POST /api/payment/process` - Process payment
- `GET /api/payment/status/{orderId}` - Check payment status

### Favorites
- `POST /api/favorites/add` - Add to favorites
- `DELETE /api/favorites/remove/{productId}` - Remove from favorites
- `GET /api/favorites` - Get favorite products

### Discounts
- `POST /api/discount/apply` - Apply discount code
- `GET /api/discount/{code}` - Get discount details

### User
- `GET /api/users/profile` - Get user profile
- `PUT /api/users/profile` - Update profile
- `GET /api/users/addresses` - Get user addresses
- `POST /api/users/addresses` - Add new address

## 🏗️ Architecture Overview

### Clean Architecture Pattern

Dự án tuân theo **Clean Architecture** pattern với các layer độc lập và dependencies hướng vào core:

```
┌─────────────────────────────────────────────────────────────┐
│           Presentation Layer (mtkpm.UI, mtkpm.Admin)         │
│              Razor Pages & Controllers                       │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│          Application Layer (mtkpm.Application)               │
│    Features, Commands, Queries, DTOs, Validators            │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│           Domain Layer (mtkpm.Domain)                        │
│      Entities, Enums, Events, Interfaces (Independent)      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│      Infrastructure Layer (mtkpm.Infrastructure)             │
│    DbContext, Repositories, Services, Migrations             │
└─────────────────────────────────────────────────────────────┘
```

### Layer Details

#### 1️⃣ **Domain Layer** (`mtkpm.Domain`) - The Core
**Độc lập hoàn toàn, không phụ thuộc vào layer khác**

**Entities** - Các thực thể chính:
- `User.cs` - Người dùng hệ thống
- `Product.cs` - Sản phẩm
- `Category.cs` - Danh mục sản phẩm
- `Order.cs` / `OrderItem.cs` - Đơn hàng và chi tiết
- `CartItem.cs` - Mục giỏ hàng
- `Discount.cs` / `DiscountUsageHistory.cs` - Khuyến mãi
- `PricingRule.cs` - Quy tắc giá
- `PaymentMethodConfig.cs` - Cấu hình phương thức thanh toán
- `UserAddress.cs` - Địa chỉ người dùng
- `FavouriteProduct.cs` - Sản phẩm yêu thích
- `RefreshToken.cs` - Token làm mới

**Base Classes**:
- `BaseEntity.cs` - Entity cơ bản (Id, CreatedAt, UpdatedAt)
- `SoftDeleteEntity.cs` - Entity với soft delete

**Enums**:
- `OrderStatus.cs` - Trạng thái đơn hàng
- `PaymentMethodType.cs` - Loại phương thức thanh toán

**Domain Events**:
- `DomainEvent.cs` - Base class cho domain events
- `OrderEvents.cs` - Các sự kiện liên quan đến đơn hàng
- `PaymentEvents.cs` - Các sự kiện thanh toán

---

#### 2️⃣ **Application Layer** (`mtkpm.Application`) - Use Cases & Business Logic

**Features** - Tổ chức theo domain/chức năng:
- **Auth/** - Xác thực
  - Commands: `RegisterCommand`, `LoginCommand`, `ChangePasswordCommand`, `RefreshTokenCommand`
  - Handlers: Xử lý logic đăng ký, đăng nhập, đổi mật khẩu
  - Validators: Validate input

- **Products/** - Quản lý sản phẩm
  - Commands: `CreateProductCommand`, `UpdateProductCommand`, `DeleteProductCommand`, `UpdateStockCommand`, `CalculatePriceCommand`
  - Queries: `GetAllProductsQuery`, `GetProductByIdQuery`, `GetProductsByCategoryQuery`, `SearchProductsQuery`, `GetProductsPaginatedQuery`

- **Categories/** - Quản lý danh mục
  - Commands: `CreateCategoryCommand`, `UpdateCategoryCommand`, `DeleteCategoryCommand`
  - Queries: `GetAllCategoriesQuery`, `GetCategoryByIdQuery`

- **Orders/** - Quản lý đơn hàng
  - Commands: `CreateOrderCommand`, `UpdateOrderStatusCommand`, `CancelOrderCommand`, `ProcessPaymentCommand`, `MarkAsPaidCommand`
  - Queries: `GetAllOrdersQuery`, `GetOrderByIdQuery`, `GetOrderByNumberQuery`, `GetUserOrdersQuery`

- **Cart/** - Quản lý giỏ hàng
  - Commands: `AddToCartCommand`, `RemoveFromCartCommand`, `UpdateCartItemCommand`, `ClearCartCommand`, `CalculateCartDiscountCommand`
  - Queries: `GetUserCartQuery`, `GetCartItemCountQuery`

- **Discounts/** - Quản lý khuyến mãi
  - Commands: `CreateDiscountCommand`, `UpdateDiscountCommand`, `DeleteDiscountCommand`
  - Queries: `GetDiscountByIdQuery`, `GetDiscountsQuery`

- **Pricing/** - Quản lý giá và quy tắc giá
  - Commands: `CreatePricingRuleCommand`, `UpdatePricingRuleCommand`, `DeletePricingRuleCommand`
  - Queries: `GetPricingRulesQuery`, `GetPricingRuleByIdQuery`

- **PaymentMethodConfigs/** - Cấu hình thanh toán
  - Commands: `CreatePaymentMethodConfigCommand`, `UpdatePaymentMethodConfigCommand`, `DeletePaymentMethodConfigCommand`
  - Queries: `GetPaymentMethodConfigsQuery`, `GetPaymentMethodConfigByCodeQuery`

- **Users/** - Quản lý người dùng
  - Commands: `UpdateUserCommand`, `AddFavouriteCommand`, `RemoveFavouriteCommand`, `CreateUserAddressCommand`, `UpdateUserAddressCommand`, `DeleteUserAddressCommand`
  - Queries: `GetUserAddressesQuery`, `GetUserFavouritesQuery`, `GetUserAddressByIdQuery`

- **NotificationMethods/** - Quản lý phương thức thông báo
  - Commands: `SubscribeNotificationMethodCommand`, `UnsubscribeNotificationMethodCommand`
  - Queries: `GetNotificationMethodsQuery`

**Common/DTOs** - Data Transfer Objects:
- Auth DTOs: `RegisterDto`, `LoginDto`, `AuthDto`, `ChangePasswordDto`, `RefreshTokenDto`
- User DTOs: `UserDto`, `UserAddressDto`, `UpdateUserDto`, `UserWithRolesDto`, `AddFavouriteProductDto`, `FavouriteProductDto`
- Product DTOs: `ProductDto`, `CreateProductDto`, `UpdateProductDto`
- Order DTOs: `OrderDto`, `OrderItemDto`, `CreateOrderDto`, `UpdateOrderStatusDto`
- Cart DTOs: `CartDto`, `CartItemDto`, `AddToCartDto`, `UpdateCartItemDto`
- Category DTOs: `CategoryDto`, `CreateCategoryDto`, `UpdateCategoryDto`
- Discount DTOs: `DiscountDto`
- Pricing DTOs: `PricingRuleDto`
- Payment DTOs: `PaymentMethodConfigDto`, `PaymentStatusInfoDto`
- Common: `ApiResponse`, `PaginatedListDto`

**Common/Interfaces** - Abstraction:
- **Repositories**: `IRepository`, `IProductRepository`, `IOrderRepository`, `ICategoryRepository`, `ICartItemRepository`, `IDiscountRepository`, `IFavouriteProductRepository`, `IUserAddressRepository`, `IPaymentMethodConfigRepository`, `IPricingRuleRepository`, `IRefreshTokenRepository`, `IUnitOfWork`
- **Services**: `IProductService`, `ICategoryService`, `ICartService`, `IOrderService`, `IPaymentService`, `IDiscountService`, `IPricingService`, `IUserService`, `IFavouriteProductService`, `INotificationMethodService`, `IPaymentMethod`, `IPaymentFactory`, `IPricingStrategy`, `INotificationObserver`
- **Auth & Token**: `IAuthService`, `IJwtService`, `ITokenService`, `ILoggerService`, `IEventPublisher`

**Mappers** - AutoMapper Profiles:
- `UserMappingProfile`
- `ProductMappingProfile`
- `OrderMappingProfile`
- `CategoryMappingProfile`
- `CartMappingProfile`
- `DiscountMappingProfile`
- `PaymentMethodConfigMappingProfile`
- `PricingRuleMappingProfile`
- `UserAddressMappingProfile`
- `MappingExtensions`

**Validators** - FluentValidation:
- Command validators cho tất cả commands
- Discount validators

**DependencyInjection.cs** - Đăng ký tất cả services

---

#### 3️⃣ **Infrastructure Layer** (`mtkpm.Infrastructure`) - Data & External Services

**Data/Contexts**:
- `ApplicationDbContext.cs` - EF Core DbContext chính

**Data/Configurations** - EF Core Configurations:
- `UserConfiguration`
- `ProductConfiguration`
- `CategoryConfiguration`
- `OrderConfiguration`
- `OrderItemConfiguration`
- `CartItemConfiguration`
- `FavouriteProductConfiguration`
- `RefreshTokenConfiguration`
- `PaymentMethodConfigConfiguration`
- `DiscountConfiguration`
- `DiscountUsageHistoryConfiguration`
- `PricingRuleConfiguration`

**Data/Repositories** - Repository Pattern Implementation:
- `Repository.cs` - Generic repository base
- `ProductRepository.cs` - Product repository
- `OrderRepository.cs` - Order repository
- `CategoryRepository.cs` - Category repository
- `CartItemRepository.cs` - Cart item repository
- `DiscountRepository.cs` - Discount repository
- `FavouriteProductRepository.cs` - Favourite product repository
- `UserAddressRepository.cs` - User address repository
- `PaymentMethodConfigRepository.cs` - Payment config repository
- `PricingRuleRepository.cs` - Pricing rule repository
- `RefreshTokenRepository.cs` - Refresh token repository

**Data/UnitOfWork**:
- `UnitOfWork.cs` - Unit of Work pattern implementation

**Data/Migrations** - EF Core Migrations:
- Multiple migration files cho database versioning

**Services/Payments** - Cổng thanh toán:
- `PaymentService.cs` - Core payment service
- `PaymentFactory.cs` - Factory pattern cho payment methods
- `CreditCardPaymentService.cs` - Thanh toán bằng thẻ tín dụng
- `PayPalPaymentService.cs` - Thanh toán PayPal
- `BankTransferPaymentService.cs` - Chuyển khoản ngân hàng
- `CODPaymentService.cs` - Thanh toán khi nhận hàng

**Services/Pricing** - Chiến lược định giá:
- `PricingService.cs` - Core pricing service
- `RegularPricingStrategy.cs` - Giá thường
- `BulkDiscountPricingStrategy.cs` - Giảm giá số lượng
- `SeasonalPricingStrategy.cs` - Giá theo mùa
- `VIPMemberPricingStrategy.cs` - Giá VIP

**Services/Discounts** - Decorator pattern cho discount:
- `DiscountService.cs` - Core discount service
- `BaseDiscount.cs` - Base class
- `DiscountDecorator.cs` - Decorator base
- `PercentageDiscountDecorator.cs` - Giảm giá phần trăm
- `FixedAmountDiscountDecorator.cs` - Giảm giá cố định
- `FreeShippingDiscountDecorator.cs` - Miễn phí vận chuyển
- `BundleDiscountDecorator.cs` - Giảm giá combo
- `LoyaltyPointsDiscountDecorator.cs` - Giảm giá điểm tích lũy

**Services/Notifications** - Hệ thống thông báo:
- `EventPublisher.cs` - Publish domain events
- `NotificationSubscriber.cs` - Subscribe to events
- `NotificationMethodService.cs` - Core notification service
- `EmailNotificationService.cs` - Email notifications
- `SMSNotificationService.cs` - SMS notifications
- `PushNotificationService.cs` - Push notifications

**Services/QRCode**:
- `QRCodeGeneratorService.cs` - QR code generation

**Services/Auth**:
- `AuthService.cs` - Authentication service
- `JwtService.cs` - JWT token management
- `CurrentUserService.cs` - Get current user info

**Services/Logging**:
- `LoggerService.cs` - Application logging

**Services/SeedData**:
- `DataSeeder.cs` - Database seeding

**Configuration**:
- `JwtSettings.cs` - JWT configuration
- `PricingRuleConfiguration.cs`
- `PaymentMethodConfigConfiguration.cs`
- `DiscountConfiguration.cs`
- `DiscountUsageHistoryConfiguration.cs`

**DependencyInjection.cs** - Đăng ký tất cả infrastructure services

---

#### 4️⃣ **Presentation Layer** (`mtkpm.UI`, `mtkpm.Admin`) - User Interfaces

**mtkpm.UI** - Customer-facing Razor Pages:
- Controllers: `HomeController`, `AuthController`
- Views: Razor pages (`.cshtml`)
- Models: `ErrorViewModel`
- Shared layouts: `_Layout.cshtml`, `_ValidationScriptsPartial.cshtml`

**mtkpm.Admin** - Admin Dashboard (.NET 10):
- Admin management interface
- Dashboard & analytics

### Design Patterns Used

✅ **Clean Architecture** - Clear separation of concerns
✅ **Repository Pattern** - Data access abstraction
✅ **Unit of Work Pattern** - Transaction management
✅ **Factory Pattern** - Payment methods creation
✅ **Strategy Pattern** - Pricing strategies
✅ **Decorator Pattern** - Discount composition
✅ **Observer Pattern** - Event notifications
✅ **CQRS** - Commands & Queries separation
✅ **DTO Pattern** - Data transfer between layers
✅ **Dependency Injection** - Loose coupling

### Data Flow Example (Order Creation)

```
1. User Request (UI Layer)
   ↓
2. Controller receives CreateOrderCommand (Presentation)
   ↓
3. Command Handler processes (Application Layer)
   ├─ Validates input with validators
   ├─ Calculates pricing using PricingStrategy
   ├─ Applies discounts using DiscountService
   ├─ Creates Order entity
   ↓
4. Repository saves to database (Infrastructure)
   ├─ Uses UnitOfWork for transactions
   ├─ Executes EF Core migrations
   ↓
5. Domain Events published
   ├─ OrderCreatedEvent
   ├─ PaymentRequiredEvent
   ↓
6. Notifications sent
   ├─ Email confirmation
   ├─ SMS notification
   └─ Push notification
   ↓
7. Response sent back to client (Presentation)
```

## 🔒 Security Features

- User authentication and authorization
- Password encryption
- Input validation
- Exception handling
- Secure payment processing
- Request logging and monitoring

## 🧪 Testing

(Add testing details as needed)

```bash
# Run unit tests
dotnet test

# Run integration tests
dotnet test --filter Category=Integration
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👨‍💻 Author

**sngkagnwho**
- GitHub: [@sngkagnwho](https://github.com/sngkagnwho)
- Repository: [Ecommercial-shop](https://github.com/sngkagnwho/Ecommercial-shop)

## 📞 Support

For support, email your-email@example.com or open an issue on GitHub.

## 🗺️ Roadmap

- [ ] Enhanced search and filtering
- [ ] Product reviews and ratings
- [ ] Wishlist features
- [ ] Multiple payment methods
- [ ] Email notifications
- [ ] SMS notifications
- [ ] Inventory management
- [ ] Advanced analytics dashboard
- [ ] Mobile app
- [ ] API documentation (Swagger/OpenAPI)

## 🙏 Acknowledgments

- .NET team for the excellent framework
- ASP.NET Core documentation
- Community contributors

---

**Last Updated:** 2024
