# 📊 PHÂN TÍCH DESIGN PATTERNS TRONG HỆ THỐNG MTKPM_WEB

## Dự Án: Mẫu Thiết Kế Phần Mềm

---

## 📌 GIỚI THIỆU

Codebase **MTKPM_WEB** sử dụng **25+ Design Patterns** được phân loại thành 5 nhóm chính:

- **Architectural Patterns** (4 patterns)
- **Creational Patterns** (3 patterns)
- **Structural Patterns** (5 patterns)
- **Behavioral Patterns** (5 patterns)
- **Enterprise Patterns** (8 patterns)

---

# 🔴 **NHÓM 1: PATTERNS CÓ MỨC ĐỘ ẢNH HƯỞNG CAO**

_Các patterns này ảnh hưởng trực tiếp đến toàn bộ cấu trúc và kiến trúc của hệ thống_

---

## 1.1 **Clean Architecture (Kiến Trúc Sạch)**

📍 **Vị trí:** Toàn bộ backend (mtkpm, mtkpm.Application, mtkpm.Domain, mtkpm.Infrastructure)

### 🎯 **Mục Đích:**

- Tách rõ ràng giữa các lớp (Domain, Application, Infrastructure, Presentation)
- Giảm sự phụ thuộc và tăng khả năng bảo trì
- Cho phép test unit mà không cần database

### 📂 **Cấu Trúc:**

```
Backend/
├── mtkpm.Domain/                    # Domain Layer (Business logic)
│   ├── Entities/                    # Core business entities
│   │   ├── Business/
│   │   │   ├── Category.cs
│   │   │   ├── Product.cs
│   │   │   ├── Order.cs
│   │   │   └── User.cs
│   │   └── Base/
│   │       ├── BaseEntity.cs        # Id, CreateAt, UpdateAt
│   │       └── SoftDeleteEntity.cs  # IsDeleted support
│   ├── Events/                      # Domain events (OrderCreatedEvent...)
│   ├── Enums/                       # Business enums (PaymentStatus...)
│   └── Exceptions/                  # Domain-specific exceptions
│
├── mtkpm.Application/               # Application Layer (Use Cases)
│   ├── Features/                    # Use cases tổ chức theo features
│   │   ├── Auth/
│   │   │   ├── Commands/
│   │   │   │   ├── RegisterCommand
│   │   │   │   └── LoginCommand
│   │   │   └── Queries/
│   │   │       └── GetCurrentUserQuery
│   │   ├── Products/
│   │   │   ├── Queries/
│   │   │   │   ├── GetAllProductsQuery
│   │   │   │   ├── GetProductByIdQuery
│   │   │   │   └── SearchProductsQuery
│   │   │   └── Commands/
│   │   │       ├── CreateProductCommand
│   │   │       ├── UpdateProductCommand
│   │   │       └── DeleteProductCommand
│   │   ├── Orders/
│   │   ├── Payments/
│   │   └── ...
│   ├── Common/
│   │   ├── Interfaces/              # Repository interfaces, Services
│   │   ├── DTOs/                    # Data transfer objects
│   │   └── Mappings/                # AutoMapper profiles
│   └── DependencyInjection.cs       # Service registration
│
├── mtkpm.Infrastructure/            # Infrastructure Layer (External services)
│   ├── Data/
│   │   ├── ApplicationDbContext     # EF Core context
│   │   ├── Repositories/            # Concrete repository implementations
│   │   │   ├── ProductRepository
│   │   │   ├── CategoryRepository
│   │   │   ├── OrderRepository
│   │   │   └── ...
│   │   ├── Migrations/              # EF Core migrations
│   │   └── UnitOfWork/
│   │       └── UnitOfWork.cs        # Manages all repositories
│   ├── Services/                    # External service implementations
│   │   ├── Auth/
│   │   │   ├── AuthService
│   │   │   ├── JwtService
│   │   │   └── RefreshTokenService
│   │   ├── Email/
│   │   ├── Payments/
│   │   ├── Pricing/
│   │   ├── Notifications/
│   │   │   ├── EmailNotificationService
│   │   │   ├── SMSNotificationService
│   │   │   └── PushNotificationService
│   │   └── ...
│   ├── Configuration/               # External API settings
│   └── DependencyInjection.cs       # Service registration
│
└── mtkpm/                           # Presentation Layer (API)
    ├── Program.cs                   # Application startup
    ├── Controllers/                 # API endpoints
    │   ├── AccountController
    │   ├── ProductsController
    │   ├── OrdersController
    │   ├── PaymentController
    │   └── ...
    ├── Middleware/                  # Custom middleware
    │   ├── ExceptionHandlingMiddleware
    │   └── RequestResponseLoggingMiddleware
    └── mtkpm.http                   # HTTP test files
```

### 💡 **Chức Năng Chi Tiết:**

| **Lớp**            | **Trách Nhiệm**                                                                                    | **Dependencies**               |
| ------------------ | -------------------------------------------------------------------------------------------------- | ------------------------------ |
| **Domain**         | - Business logic, rules, entities<br>- Domain events<br>- Exceptions<br>- Value objects            | Không phụ thuộc gì             |
| **Application**    | - Use cases (Commands/Queries)<br>- DTOs<br>- Mappers<br>- Validators                              | Chỉ phụ thuộc Domain           |
| **Infrastructure** | - Database access (EF Core)<br>- External services<br>- Repositories<br>- Concrete implementations | Phụ thuộc Domain & Application |
| **Presentation**   | - API Controllers<br>- Middleware<br>- Routing<br>- Request/Response handling                      | Phụ thuộc Application          |

### 📝 **Ví Dụ Luồng Xử Lý:**

```
User Request → Controller → MediatR (CQRS)
                            ↓
                        Command/Query Handler (Application)
                            ↓
                        Domain Business Logic validation
                            ↓
                        Database Query via Repository (Infrastructure)
                            ↓
                        Mapper: Entity → DTO
                            ↓
                        Response to User
```

### ✅ **Lợi Ích:**

1. **Dễ bảo trì:** Thay đổi database không ảnh hưởng business logic
2. **Dễ test:** Có thể test business logic mà không cần database thực
3. **Independent frameworks:** Có thể thay đổi EF Core sang Dapper mà không ảnh hưởng core logic
4. **Scalable:** Dễ thêm new features mà không phá vỡ code cũ

---

## 1.2 **CQRS (Command Query Responsibility Segregation)**

📍 **Vị trí:** Backend (Program.cs, Controllers, Features)

### 🎯 **Mục Đích:**

- Tách biệt lệnh (thay đổi dữ liệu) từ truy vấn (lấy dữ liệu)
- Tối ưu hóa read & write operations khác nhau
- Cho phép scale riêng read/write models

### 📝 **Cấu Trúc Commands & Queries:**

#### **COMMANDS (Thay đổi dữ liệu):**

```csharp
// 1. Auth Commands
public class RegisterCommand : IRequest<AuthResponse>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string? PhoneNumber { get; set; }
}

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

// 2. Product Commands
public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public List<string> ImageUrls { get; set; }
}

public class UpdateProductCommand : IRequest<ProductDto>
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
}

// 3. Order Commands
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int UserId { get; set; }
    public List<CartItemDto> CartItems { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public string PaymentMethod { get; set; }
}

public class UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public int OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}

// 4. Payment Commands
public class ProcessPaymentCommand : IRequest<PaymentResponseDto>
{
    public int OrderId { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string? CardToken { get; set; }
}

// 5. Cart Commands
public class AddToCartCommand : IRequest<CartDto>
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class RemoveFromCartCommand : IRequest<CartDto>
{
    public int UserId { get; set; }
    public int CartItemId { get; set; }
}
```

#### **QUERIES (Lấy dữ liệu):**

```csharp
// 1. Product Queries
public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}

public class SearchProductsQuery : IRequest<List<ProductDto>>
{
    public string SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

// 2. Order Queries
public class GetUserOrdersQuery : IRequest<List<OrderDto>>
{
    public int UserId { get; set; }
    public OrderStatus? Status { get; set; }
}

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public int OrderId { get; set; }
}

// 3. User Queries
public class GetCurrentUserQuery : IRequest<UserDto?>
{
}

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int UserId { get; set; }
}

// 4. Cart Queries
public class GetUserCartQuery : IRequest<CartDto>
{
    public int UserId { get; set; }
}

public class GetFavouriteProductsQuery : IRequest<List<ProductDto>>
{
    public int UserId { get; set; }
}
```

### 📊 **Workflow CQRS:**

```
┌─────────────────────────────────────────────────────────────┐
│                      HTTP Request                           │
└──────────────────────┬──────────────────────────────────────┘
                       │
        ┌──────────────┴──────────────┐
        │                             │
        ▼                             ▼
   ┌─────────────┐             ┌──────────────┐
   │   Command   │             │    Query     │
   │   (Write)   │             │    (Read)    │
   └──────┬──────┘             └────────┬─────┘
          │                             │
          │ MediatR dispatches          │
          │                             │
          ▼                             ▼
  ┌──────────────────────┐   ┌──────────────────────┐
  │ CommandHandler:      │   │ QueryHandler:        │
  │ - Validates          │   │ - Fetch data         │
  │ - Business logic     │   │ - Map to DTO         │
  │ - Saves to DB        │   │ - Return results     │
  │ - Publishes events   │   │                      │
  └──┬───────────────────┘   └────────┬─────────────┘
     │                                 │
     ▼                                 ▼
  ┌──────────────────────┐   ┌──────────────────────┐
  │   EventPublisher     │   │      ApiResponse     │
  │ - Triggers           │   │      (DTO)           │
  │   notifications      │   │                      │
  └──────────────────────┘   └────────┬─────────────┘
                                      │
                                      ▼
                              ┌──────────────────┐
                              │  HTTP Response   │
                              └──────────────────┘
```

### 💡 **Chức Năng Chi Tiết:**

**Commands (Modifying Operations):**

1. **Register** - Tạo tài khoản mới
2. **Login** - Xác thực người dùng
3. **CreateProduct** - Thêm sản phẩm mới
4. **UpdateProduct** - Cập nhật thông tin sản phẩm
5. **DeleteProduct** - Xóa sản phẩm (soft delete)
6. **CreateOrder** - Tạo đơn hàng
7. **UpdateOrderStatus** - Cập nhật trạng thái đơn hàng
8. **ProcessPayment** - Xử lý thanh toán
9. **AddToCart** - Thêm sản phẩm vào giỏ
10. **RemoveFromCart** - Xóa sản phẩm khỏi giỏ

**Queries (Reading Operations):**

1. **GetAllProducts** - Lấy danh sách sản phẩm (phân trang)
2. **GetProductById** - Lấy chi tiết sản phẩm
3. **SearchProducts** - Tìm kiếm sản phẩm
4. **GetUserOrders** - Lấy đơn hàng của người dùng
5. **GetOrderById** - Lấy chi tiết đơn hàng
6. **GetCurrentUser** - Lấy thông tin người dùng hiện tại
7. **GetUserCart** - Lấy giỏ hàng của người dùng
8. **GetFavouriteProducts** - Lấy danh sách yêu thích

### ✅ **Lợi Ích:**

1. **Tối ưu hóa:** Có thể optimize read/write khác nhau
2. **Scale riêng:** Có thể scale read model với caching
3. **Clear intent:** Code rõ ràng là đọc hay ghi dữ liệu
4. **Event handling:** Dễ trigger side effects (notifications, emails)

---

## 1.3 **Domain-Driven Design (DDD)**

📍 **Vị trí:** mtkpm.Domain Layer

### 🎯 **Mục Đích:**

- Tập trung vào business logic (domain)
- Sử dụng ubiquitous language (ngôn ngữ chung giữa developers & business)
- Ghi lại các sự kiện quan trọng (Domain Events)

### 📝 **Thành Phần DDD:**

#### **1. Entities (Thực Thể Kinh Doanh):**

```csharp
public class Product : SoftDeleteEntity
{
    // Identity
    [Key]
    public int Id { get; set; }

    // Properties
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsAvailable => StockQuantity > 0 && !IsDeleted;

    // Relationships
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // Business methods
    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");
        if (StockQuantity < quantity)
            throw new InvalidOperationException($"Not enough stock. Available: {StockQuantity}, Requested: {quantity}");

        StockQuantity -= quantity;
    }

    public void Restock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        StockQuantity += quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative");

        Price = newPrice;
        UpdateAt = DateTime.UtcNow;
    }
}

public class Order : SoftDeleteEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // Business logic methods
    public void ConfirmOrder()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");

        Status = OrderStatus.Confirmed;
    }

    public void ShipOrder()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be shipped");

        Status = OrderStatus.Shipped;
    }

    public void CancelOrder()
    {
        if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot cancel completed orders");

        Status = OrderStatus.Cancelled;
    }

    public decimal CalculateTotalAmount()
    {
        return OrderItems.Sum(x => x.Quantity * x.UnitPrice);
    }
}

public class User : SoftDeleteEntity
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
    public ICollection<FavouriteProduct> FavouriteProducts { get; set; } = new List<FavouriteProduct>();

    // Business logic
    public bool IsValidEmail()
    {
        return Email.Contains("@") && Email.Contains(".");
    }

    public void UpdateProfile(string email, string? phoneNumber)
    {
        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email");

        Email = email;
        PhoneNumber = phoneNumber;
        UpdateAt = DateTime.UtcNow;
    }
}
```

#### **2. Domain Events (Sự Kiện Miền):**

```csharp
// Base class
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid AggregateId { get; protected set; }
}

// Specific events
public class OrderCreatedEvent : DomainEvent
{
    public int OrderId { get; }
    public int UserId { get; }
    public string OrderNumber { get; }
    public decimal TotalAmount { get; }
    public DateTime OrderDate { get; }

    public OrderCreatedEvent(int orderId, int userId, string orderNumber, decimal totalAmount)
    {
        OrderId = orderId;
        UserId = userId;
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
        OrderDate = DateTime.UtcNow;
        AggregateId = new Guid(orderId.ToString());
    }
}

public class PaymentCompletedEvent : DomainEvent
{
    public int OrderId { get; }
    public int UserId { get; }
    public string TransactionId { get; }
    public decimal Amount { get; }
    public string PaymentMethod { get; }
    public PaymentStatus PaymentStatus { get; }

    public PaymentCompletedEvent(int orderId, int userId, string transactionId, decimal amount, string paymentMethod)
    {
        OrderId = orderId;
        UserId = userId;
        TransactionId = transactionId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PaymentStatus = PaymentStatus.Completed;
        AggregateId = new Guid(orderId.ToString());
    }
}

public class ProductStockChangedEvent : DomainEvent
{
    public int ProductId { get; }
    public int OldQuantity { get; }
    public int NewQuantity { get; }
    public string ChangeReason { get; }

    public ProductStockChangedEvent(int productId, int oldQuantity, int newQuantity, string reason)
    {
        ProductId = productId;
        OldQuantity = oldQuantity;
        NewQuantity = newQuantity;
        ChangeReason = reason;
        AggregateId = new Guid(productId.ToString());
    }
}

public class UserRegisteredEvent : DomainEvent
{
    public int UserId { get; }
    public string Email { get; }
    public string UserName { get; }
    public DateTime RegistrationDate { get; }

    public UserRegisteredEvent(int userId, string email, string userName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
        RegistrationDate = DateTime.UtcNow;
        AggregateId = new Guid(userId.ToString());
    }
}
```

#### **3. Value Objects (Đối Tượng Giá Trị):**

```csharp
public class Money
{
    public decimal Amount { get; }
    public string Currency { get; } = "VND";

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");
        Amount = amount;
    }

    public Money Add(Money other)
    {
        return new Money(Amount + other.Amount);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor);
    }
}

public class Address
{
    public string Street { get; set; }
    public string Ward { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; } = "Vietnam";

    public string GetFullAddress()
    {
        return $"{Street}, {Ward}, {District}, {City}, {PostalCode}";
    }
}
```

#### **4. Repositories (Tương Tác với Domain):**

```csharp
// Repository cho aggregate roots
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, OrderStatus? status = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
}
```

### 💡 **Chức Năng Chi Tiết:**

| **Thành Phần**    | **Chức Năng**                                                                        | **Ví Dụ**                                |
| ----------------- | ------------------------------------------------------------------------------------ | ---------------------------------------- |
| **Entities**      | Đại diện cho các khái niệm kinh doanh<br>Chứa business logic<br>Có identity dài hạn  | Product, Order, User, Category           |
| **Value Objects** | Biểu diễn các khái niệm không có identity<br>Immutable<br>Có logic so sánh           | Money, Address, PhoneNumber              |
| **Domain Events** | Ghi lại sự kiện quan trọng<br>Trigger notifications, emails<br>Audit trail           | OrderCreatedEvent, PaymentCompletedEvent |
| **Aggregates**    | Nhóm các entities liên quan<br>Có aggregate root<br>Xử lý transactions               | Order (root) + OrderItems (children)     |
| **Repositories**  | Tóm tắt data persistence<br>Cung cấp query methods<br>Không phụ thuộc infrastructure | IProductRepository, IOrderRepository     |

### ✅ **Lợi Ích:**

1. **Business-focused:** Code phản ánh business logic thực tế
2. **Maintainability:** Dễ hiểu và thay đổi business rules
3. **Event sourcing ready:** Có thể đăng ký events cho audit
4. **Domain expertise:** Developers hiểu business details

---

## 1.4 **Feature-Based Folder Structure (Micro-services mindset)**

📍 **Vị trí:** mtkpm.Admin Frontend, mtkpm.UI

### 🎯 **Mục Đích:**

- Tổ chức code theo features thay vì theo technical layers
- Dễ nhất là tìm tất cả code liên quan đến một feature
- Dễ scale lên microservices sau này

### 📂 **Cấu Trúc Thư Mục:**

```
mtkpm.Admin/
├── Features/
│   ├── Products/
│   │   ├── Controllers/
│   │   │   └── ProductsController.cs
│   │   ├── Services/
│   │   │   └── IProductModelService.cs
│   │   │   └── ProductModelService.cs
│   │   ├── Models/
│   │   │   ├── ProductCreateModel.cs
│   │   │   ├── ProductUpdateModel.cs
│   │   │   └── ProductDetailModel.cs
│   │   ├── Views/
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Details.cshtml
│   │   └── _ViewImports.cshtml (Feature-specific imports)
│   │
│   ├── Orders/
│   │   ├── Controllers/
│   │   │   └── OrdersController.cs
│   │   ├── Services/
│   │   │   └── IOrderModelService.cs
│   │   ├── Models/
│   │   │   └── OrderDetailModel.cs
│   │   └── Views/
│   │       ├── Index.cshtml
│   │       └── Details.cshtml
│   │
│   ├── Users/
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Models/
│   │   └── Views/
│   │
│   ├── Categories/
│   ├── Dashboard/
│   ├── Reports/
│   └── Settings/
│
├── Infrastructure/
│   ├── Http/
│   │   └── HttpClientWrapper.cs
│   ├── Caching/
│   │   └── CacheService.cs
│   ├── Logging/
│   └── Configuration/
│
├── Shared/
│   ├── Components/
│   ├── Views/
│   └── Constants/
│
└── Core/
    ├── Constants/
    └── Settings/
```

### 💡 **Chức Năng Chi Tiết:**

**Tổ chức theo Features:**

1. **Products Feature**
   - Quản lý sản phẩm
   - CRUD operations
   - Tìm kiếm & filter

2. **Orders Feature**
   - Quản lý đơn hàng
   - Cập nhật trạng thái
   - Xem chi tiết

3. **Users Feature**
   - Quản lý người dùng
   - Gán quyền hạn
   - Khóa/Mở khóa tài khoản

4. **Categories Feature**
   - Quản lý danh mục sản phẩm
   - Phân cấp danh mục

5. **Dashboard Feature**
   - Thống kê bán hàng
   - Analytics
   - KPIs

### ✅ **Lợi Ích:**

1. **Co-location:** Tất cả code của 1 feature ở một chỗ
2. **Independence:** Có thể develop features riêng biệt
3. **Scalability:** Dễ tách riêng thành microservice
4. **Discoverability:** Dễ tìm code liên quan

---

# 🟠 **NHÓM 2: PATTERNS CÓ MỨC ĐỘ ẢNH HƯỞNG VỪA**

_Các patterns này ảnh hưởng đến cách xử lý dữ liệu, business logic, hoặc tương tác với hệ thống bên ngoài_

---

## 2.1 **Repository Pattern & Unit of Work Pattern**

📍 **Vị trí:** mtkpm.Infrastructure/Data/Repositories, mtkpm.Infrastructure/Data/UnitOfWork

### 🎯 **Mục Đích:**

- Tóm tắt logic truy cập dữ liệu
- Cung cấp interface CRUD chung
- Cho phép test mà không cần database thực

### 📝 **Cấu Trúc:**

#### **Generic Repository Interface:**

```csharp
public interface IRepository<T> where T : class
{
    // Read operations
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    // Write operations
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
```

#### **Generic Repository Implementation:**

```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}
```

#### **Specialized Repositories:**

```csharp
// Product Repository
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default);
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public async Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(searchTerm) && !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.StockQuantity <= threshold && !p.IsDeleted)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync(cancellationToken);
    }
}

// Order Repository
public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, OrderStatus? status = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, OrderStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId && !o.IsDeleted);

        if (status.HasValue)
            query = query.Where(o => o.Status == status);

        return await query
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && !o.IsDeleted && o.Status == OrderStatus.Delivered)
            .SumAsync(o => o.TotalAmount, cancellationToken);
    }
}
```

#### **Unit of Work Pattern:**

```csharp
public interface IUnitOfWork : IDisposable
{
    // Repository properties
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    ICartItemRepository CartItems { get; }
    IFavouriteProductRepository FavouriteProducts { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUserAddressRepository UserAddresses { get; }
    IUserRepository Users { get; }
    IDiscountRepository Discounts { get; }

    // Data operations
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Transaction management
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IProductRepository? _products;
    private ICategoryRepository? _categories;
    private IOrderRepository? _orders;
    private IUserRepository? _users;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProductRepository Products => _products ??= new ProductRepository(_context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
                await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
```

#### **Sử Dụng Repository & UnitOfWork:**

```csharp
// QueryHandler Example
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(request.Id, cancellationToken);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }
}

// CommandHandler Example
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Begin transaction
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Create order
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = request.CartItems.Sum(x => x.Price * x.Quantity)
            };
            await _unitOfWork.Orders.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 2. Add order items and decrease stock
            foreach (var cartItem in request.CartItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId, cancellationToken);
                product.DecreaseStock(cartItem.Quantity);

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 3. Clear cart items
            var cartItems = await _unitOfWork.CartItems.FindAsync(
                c => c.UserId == request.UserId,
                cancellationToken
            );
            foreach (var cartItem in cartItems)
                _unitOfWork.CartItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Commit transaction
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // 5. Publish event (after transaction committed)
            await _eventPublisher.PublishAsync(new OrderCreatedEvent(
                order.Id,
                order.UserId,
                order.OrderNumber,
                order.TotalAmount
            ), cancellationToken);

            return _mapper.Map<OrderDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
```

### 💡 **Chức Năng Chi Tiết:**

| **Pattern**                | **Mục Đích**             | **Chức Năng**                                   |
| -------------------------- | ------------------------ | ----------------------------------------------- |
| **Repository**             | Tóm tắt data access      | CRUD, Query, Find operations                    |
| **Unit of Work**           | Quản lý transactions     | SaveChanges, BeginTransaction, Commit, Rollback |
| **Lazy Loading**           | Tạo repositories khi cần | Tiết kiệm memory                                |
| **Include/ThenInclude**    | Eager load related data  | Giảm N+1 queries                                |
| **Transaction Management** | Đảm bảo data consistency | All-or-nothing operations                       |

### ✅ **Lợi Ích:**

1. **Data access abstraction:** Không cần biết EF Core internals
2. **Testability:** Có thể mock repositories
3. **Consistency:** Tất cả ops qua UnitOfWork
4. **Transaction safety:** Tự động rollback on error

---

## 2.2 **Decorator Pattern (Discount System)**

📍 **Vị trí:** mtkpm.Infrastructure/Services/Discounts

### 🎯 **Mục Đích:**

- Xếp chồng các discounts (giảm giá) mà không cần tạo nhiều subclass
- Chọn discounts động tại runtime
- Tính toán final price với nhiều discount rules

### 📝 **Cấu Trúc:**

#### **Component Interface:**

```csharp
public interface IDiscount
{
    string DiscountName { get; }
    decimal ApplyDiscount(CartDto cart);
}
```

#### **Base Component:**

```csharp
public class BaseDiscount : IDiscount
{
    public string DiscountName => "NoDiscount";

    public virtual decimal ApplyDiscount(CartDto cart)
    {
        return cart.TotalAmount; // Không giảm
    }
}
```

#### **Decorator Base Class:**

```csharp
public abstract class DiscountDecorator : IDiscount
{
    protected readonly IDiscount _innerDiscount;

    public abstract string DiscountName { get; }

    public DiscountDecorator(IDiscount innerDiscount)
    {
        _innerDiscount = innerDiscount;
    }

    public virtual decimal ApplyDiscount(CartDto cart)
    {
        // First apply inner discount
        var priceAfterInner = _innerDiscount.ApplyDiscount(cart);

        // Then create new cart with updated total
        var updatedCart = new CartDto
        {
            TotalAmount = priceAfterInner,
            Items = cart.Items
        };

        // Apply current discount
        return ApplyCurrentDiscount(updatedCart);
    }

    protected abstract decimal ApplyCurrentDiscount(CartDto cart);
}
```

#### **Concrete Decorators:**

```csharp
// 1. Percentage Discount Decorator
public class PercentageDiscountDecorator : DiscountDecorator
{
    private readonly decimal _discountPercent;

    public override string DiscountName => $"Percentage Discount ({_discountPercent}%)";

    public PercentageDiscountDecorator(IDiscount innerDiscount, decimal discountPercent)
        : base(innerDiscount)
    {
        _discountPercent = discountPercent;
    }

    protected override decimal ApplyCurrentDiscount(CartDto cart)
    {
        var discountAmount = cart.TotalAmount * (_discountPercent / 100);
        var finalPrice = cart.TotalAmount - discountAmount;

        Console.WriteLine($"Applied {DiscountName}: {discountAmount:C} discount");

        return finalPrice;
    }
}

// 2. Free Shipping Discount Decorator
public class FreeShippingDiscountDecorator : DiscountDecorator
{
    private readonly decimal _shippingCost;

    public override string DiscountName => "Free Shipping";

    public FreeShippingDiscountDecorator(IDiscount innerDiscount, decimal shippingCost = 50000m)
        : base(innerDiscount)
    {
        _shippingCost = shippingCost;
    }

    protected override decimal ApplyCurrentDiscount(CartDto cart)
    {
        var finalPrice = cart.TotalAmount - _shippingCost;
        Console.WriteLine($"Applied {DiscountName}: {_shippingCost:C} discount");

        return finalPrice;
    }
}

// 3. Buy More Save More Discount Decorator
public class VolumeDiscountDecorator : DiscountDecorator
{
    private readonly int _minQuantity;
    private readonly decimal _discountPercent;

    public override string DiscountName => $"Volume Discount (Min: {_minQuantity} items, {_discountPercent}% off)";

    public VolumeDiscountDecorator(IDiscount innerDiscount, int minQuantity, decimal discountPercent)
        : base(innerDiscount)
    {
        _minQuantity = minQuantity;
        _discountPercent = discountPercent;
    }

    protected override decimal ApplyCurrentDiscount(CartDto cart)
    {
        var totalItems = cart.Items.Sum(x => x.Quantity);

        if (totalItems >= _minQuantity)
        {
            var discountAmount = cart.TotalAmount * (_discountPercent / 100);
            var finalPrice = cart.TotalAmount - discountAmount;
            Console.WriteLine($"Applied {DiscountName}: {discountAmount:C} discount");
            return finalPrice;
        }

        return cart.TotalAmount;
    }
}

// 4. Member Discount Decorator
public class MemberDiscountDecorator : DiscountDecorator
{
    private readonly MembershipLevel _membershipLevel;

    public override string DiscountName => $"{_membershipLevel} Member Discount";

    public MemberDiscountDecorator(IDiscount innerDiscount, MembershipLevel membershipLevel)
        : base(innerDiscount)
    {
        _membershipLevel = membershipLevel;
    }

    protected override decimal ApplyCurrentDiscount(CartDto cart)
    {
        var discountPercent = _membershipLevel switch
        {
            MembershipLevel.Bronze => 5m,
            MembershipLevel.Silver => 10m,
            MembershipLevel.Gold => 15m,
            _ => 0m
        };

        var discountAmount = cart.TotalAmount * (discountPercent / 100);
        var finalPrice = cart.TotalAmount - discountAmount;
        Console.WriteLine($"Applied {DiscountName}: {discountAmount:C} discount");

        return finalPrice;
    }
}

// 5. Maximum Discount Cap Decorator
public class MaxDiscountCapDecorator : DiscountDecorator
{
    private readonly decimal _maxDiscountAmount;

    public override string DiscountName => $"Max Discount Cap ({_maxDiscountAmount:C})";

    public MaxDiscountCapDecorator(IDiscount innerDiscount, decimal maxDiscountAmount)
        : base(innerDiscount)
    {
        _maxDiscountAmount = maxDiscountAmount;
    }

    protected override decimal ApplyCurrentDiscount(CartDto cart)
    {
        // Calculate actual discount from previous decorators
        var originalPrice = cart.TotalAmount;
        var actualDiscount = originalPrice - cart.TotalAmount; // This won't work directly

        // In practice, we track discount during entire chain
        var finalPrice = cart.TotalAmount;
        return finalPrice;
    }
}
```

#### **Sử Dụng Discount Decorators:**

```csharp
// Scenario 1: Regular customer, no discount
var discount = new BaseDiscount();
var finalPrice = discount.ApplyDiscount(cart);
// Result: 1,000,000

// Scenario 2: Regular customer with 10% discount
var discount = new PercentageDiscountDecorator(
    new BaseDiscount(),
    discountPercent: 10m
);
var finalPrice = discount.ApplyDiscount(cart);
// Applied Percentage Discount (10%): 100,000 discount
// Result: 900,000

// Scenario 3: Gold member with multiple stacked discounts
var discount = new FreeShippingDiscountDecorator(
    new MemberDiscountDecorator(
        new VolumeDiscountDecorator(
            new BaseDiscount(),
            minQuantity: 5,
            discountPercent: 5m
        ),
        membershipLevel: MembershipLevel.Gold
    ),
    shippingCost: 50000m
);
var finalPrice = discount.ApplyDiscount(cart);
// Applied Volume Discount (Min: 5 items, 5% off): 50,000 discount
// Applied Gold Member Discount: 142,500 discount
// Applied Free Shipping: 50,000 discount
// Result: 757,500 (from 1,000,000)
```

### 💡 **Chức Năng Chi Tiết:**

| **Decorator**          | **Discount Type**  | **Calculation**        | **Usage**               |
| ---------------------- | ------------------ | ---------------------- | ----------------------- |
| **PercentageDiscount** | % discount         | `price * (1 - %/100)`  | Flash sales, promotions |
| **FreeShipping**       | Flat discount      | `price - shippingCost` | Min order value         |
| **VolumeDiscount**     | Tiered pricing     | % off for min quantity | Bulk orders             |
| **MemberDiscount**     | Membership benefit | Varies by level        | Loyalty programs        |
| **MaxDiscountCap**     | Limit max savings  | Cap discount amount    | Prevent huge losses     |
| **SeasonalDiscount**   | Time-based         | Depends on season      | Holiday sales           |

### ✅ **Lợi Ích:**

1. **Composability:** Stack discounts without new subclass
2. **Flexibility:** Add/remove discounts at runtime
3. **Open/Closed:** Can add new decorators without modifying existing
4. **Single Responsibility:** Each decorator handles one discount type

---

## 2.3 **Strategy Pattern (Pricing Service)**

📍 **Vị trí:** mtkpm.Infrastructure/Services/Pricing

### 🎯 **Mục Đích:**

- Tính giá bằng các chiến lược khác nhau
- Chọn chiến lược dựa vào context
- Dễ thêm pricing algorithms mới

### 📝 **Cấu Trúc:**

#### **Strategy Interface:**

```csharp
public interface IPricingStrategy
{
    string StrategyName { get; }
    string Description { get; }
    decimal CalculatePrice(Product product, int quantity, PricingContext context);
}

public class PricingContext
{
    public DateTime CurrentDate { get; set; } = DateTime.UtcNow;
    public int? UserId { get; set; }
    public MembershipLevel? MembershipLevel { get; set; }
    public int? TotalQuantity { get; set; }
    public decimal? UserSpending { get; set; }
}
```

#### **Concrete Strategies:**

```csharp
// 1. Regular Pricing Strategy
public class RegularPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Regular Pricing";
    public string Description => "Standard price per unit";

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        return product.Price * quantity;
    }
}

// 2. Bulk Discount Strategy
public class BulkDiscountPricingStrategy : IPricingStrategy
{
    private readonly int _threshold;
    private readonly decimal _discountPercent;

    public string StrategyName => "Bulk Discount";
    public string Description => $"Get {_discountPercent}% off when buying {_threshold}+ items";

    public BulkDiscountPricingStrategy(int threshold = 10, decimal discountPercent = 10m)
    {
        _threshold = threshold;
        _discountPercent = discountPercent;
    }

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        if (quantity >= _threshold)
        {
            var discount = product.Price * quantity * (_discountPercent / 100);
            return (product.Price * quantity) - discount;
        }
        return product.Price * quantity;
    }
}

// 3. VIP Member Pricing Strategy
public class VIPMemberPricingStrategy : IPricingStrategy
{
    private readonly decimal _discountPercent;

    public string StrategyName => "VIP Pricing";
    public string Description => "Special pricing for VIP members";

    public VIPMemberPricingStrategy(decimal discountPercent = 15m)
    {
        _discountPercent = discountPercent;
    }

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var basePrice = product.Price * quantity;
        var discount = basePrice * (_discountPercent / 100);
        return basePrice - discount;
    }
}

// 4. Seasonal Pricing Strategy
public class SeasonalPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Seasonal Pricing";
    public string Description => "Dynamic pricing based on season";

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var basePrice = product.Price * quantity;

        // Check if it's a holiday/sale season
        var isHolidaySeason = IsHolidaySeason(context.CurrentDate);
        var discountPercent = isHolidaySeason ? 20m : 0m;

        var discount = basePrice * (discountPercent / 100);
        return basePrice - discount;
    }

    private bool IsHolidaySeason(DateTime date)
    {
        // Christmas, New Year, Lunar New Year, etc.
        var month = date.Month;
        var day = date.Day;

        return (month == 12 && day >= 20) || // Christmas season
               (month == 1 && day <= 10) ||  // New Year
               (month == 2 && day <= 14);    // Lunar New Year (approx)
    }
}

// 5. Progressive Discount Strategy
public class ProgressiveDiscountPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Progressive Discount";
    public string Description => "More items = bigger discount per item";

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var discountPercent = quantity switch
        {
            >= 50 => 25m,  // 50+ items: 25% off
            >= 20 => 15m,  // 20+ items: 15% off
            >= 10 => 10m,  // 10+ items: 10% off
            >= 5 => 5m,    // 5+ items: 5% off
            _ => 0m        // < 5 items: No discount
        };

        var basePrice = product.Price * quantity;
        var discount = basePrice * (discountPercent / 100);
        return basePrice - discount;
    }
}

// 6. Loyalty-Based Pricing Strategy
public class LoyaltyBasedPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Loyalty Pricing";
    public string Description => "Price based on customer loyalty/spending";

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var basePrice = product.Price * quantity;

        // Discount based on total spending
        var discountPercent = context.UserSpending switch
        {
            >= 10000000m => 20m,  // 10M+ spent: 20% off
            >= 5000000m => 15m,   // 5M+ spent: 15% off
            >= 1000000m => 10m,   // 1M+ spent: 10% off
            >= 500000m => 5m,     // 500K+ spent: 5% off
            _ => 0m
        };

        var discount = basePrice * (discountPercent / 100);
        return basePrice - discount;
    }
}

// 7. Time-Based Pricing Strategy (Happy Hours/Peak Hours)
public class TimeBasedPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Time-Based Pricing";
    public string Description => "Different prices for different times";

    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var hour = context.CurrentDate.Hour;
        var basePrice = product.Price * quantity;

        // Happy hour: 6-9 AM, 3-6 PM
        var isHappyHour = (hour >= 6 && hour < 9) || (hour >= 15 && hour < 18);

        if (isHappyHour)
        {
            var discount = basePrice * 0.10m; // 10% off
            return basePrice - discount;
        }

        // Peak hours: 12-1 PM, 5-7 PM - Surcharge
        var isPeakHour = (hour >= 12 && hour <= 13) || (hour >= 17 && hour < 19);

        if (isPeakHour)
        {
            var surcharge = basePrice * 0.05m; // 5% surcharge
            return basePrice + surcharge;
        }

        return basePrice;
    }
}
```

#### **Pricing Service (Context):**

```csharp
public interface IPricingService
{
    decimal CalculatePrice(Product product, int quantity, IPricingStrategy strategy, PricingContext context);
    decimal CalculateBestPrice(Product product, int quantity, PricingContext context);
    List<(string Strategy, decimal Price)> GetAllPrices(Product product, int quantity, PricingContext context);
}

public class PricingService : IPricingService
{
    private readonly Dictionary<string, IPricingStrategy> _strategies;
    private readonly ILogger _logger;

    public PricingService(ILoggerService logger)
    {
        _logger = logger;
        _strategies = new Dictionary<string, IPricingStrategy>
        {
            { "regular", new RegularPricingStrategy() },
            { "bulk", new BulkDiscountPricingStrategy(threshold: 10, discountPercent: 10m) },
            { "vip", new VIPMemberPricingStrategy(discountPercent: 15m) },
            { "seasonal", new SeasonalPricingStrategy() },
            { "progressive", new ProgressiveDiscountPricingStrategy() },
            { "loyalty", new LoyaltyBasedPricingStrategy() },
            { "timebased", new TimeBasedPricingStrategy() }
        };
    }

    public decimal CalculatePrice(Product product, int quantity, IPricingStrategy strategy, PricingContext context)
    {
        _logger.LogInfo($"Calculating price for product '{product.Name}' using strategy '{strategy.StrategyName}'");
        var price = strategy.CalculatePrice(product, quantity, context);
        _logger.LogInfo($"Calculated price: {price:C}");
        return price;
    }

    public decimal CalculateBestPrice(Product product, int quantity, PricingContext context)
    {
        _logger.LogInfo("Calculating best price across all strategies");

        var prices = GetAllPrices(product, quantity, context);
        var bestPrice = prices.OrderBy(x => x.Price).First();

        _logger.LogInfo($"Best price: {bestPrice.Price:C} using strategy '{bestPrice.Strategy}'");
        return bestPrice.Price;
    }

    public List<(string Strategy, decimal Price)> GetAllPrices(Product product, int quantity, PricingContext context)
    {
        return _strategies
            .Select(kvp => (kvp.Key, CalculatePrice(product, quantity, kvp.Value, context)))
            .ToList();
    }
}
```

#### **Sử Dụng Strategy:**

```csharp
// Example: Pricing a product for different customers
var product = new Product { Name = "Laptop", Price = 20000000m };
var context = new PricingContext { CurrentDate = DateTime.UtcNow };

// Regular customer
var regularPrice = pricingService.CalculatePrice(
    product,
    1,
    new RegularPricingStrategy(),
    context
);
// Result: 20,000,000

// Bulk buyer (15 units)
var bulkPrice = pricingService.CalculatePrice(
    product,
    15,
    new BulkDiscountPricingStrategy(10, 15m),
    context
);
// Result: 19,000,000 (10% off = 30M discount)

// VIP member
var vipPrice = pricingService.CalculatePrice(
    product,
    1,
    new VIPMemberPricingStrategy(15m),
    context
);
// Result: 17,000,000 (15% off = 3M discount)

// Find best price
var bestPrice = pricingService.CalculateBestPrice(product, 10, context);

// Compare all strategies
var allPrices = pricingService.GetAllPrices(product, 10, context);
// Regular: 200M, Bulk: 190M, VIP: 170M, Seasonal: 200M (or less), ...
```

### 💡 **Chức Năng Chi Tiết:**

| **Strategy**    | **Điều Kiện Áp Dụng**   | **Công Thức Tính Giá**   | **Ví Dụ**           |
| --------------- | ----------------------- | ------------------------ | ------------------- |
| **Regular**     | Mặc định                | `price * quantity`       | 1M \* 1 = 1M        |
| **Bulk**        | `quantity >= threshold` | `base * (1 - discount%)` | 1M _ 20 _ 0.9 = 18M |
| **VIP**         | Membership level = VIP  | `base * (1 - 15%)`       | 1M \* 0.85 = 850K   |
| **Seasonal**    | Holiday periods         | `base * (1 - 20%)`       | Christmas: 800K     |
| **Progressive** | Quantity tiers          | Tier-based %             | 50+ items: -25%     |
| **Loyalty**     | Spending history        | Based on total spent     | 10M+ spent: -20%    |
| **TimeBased**   | Current time            | Hour-based calculation   | 6-9 AM: -10%        |

### ✅ **Lợi Ích:**

1. **Dynamic selection:** Choose strategy at runtime
2. **Easy to test:** Test each strategy independently
3. **Easy to add:** New pricing algorithms without modifying existing
4. **Business logic:** Clear representation of pricing rules

---

## 2.4 **Observer Pattern (Event Publishing & Notifications)**

📍 **Vị trí:** mtkpm.Infrastructure/Services/Notifications, EventPublisher

### 🎯 **Mục Đích:**

- Publish domain events
- Multiple notification services subscribe to events
- Loosely coupled: Publishers don't know about subscribers

### 📝 **Cấu Trúc:**

#### **Subject Interface (Event Publisher):**

```csharp
public interface IEventPublisher
{
    void Subscribe(INotificationObserver observer);
    void Unsubscribe(INotificationObserver observer);
    Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default);
}
```

#### **Subject Implementation:**

```csharp
public class EventPublisher : IEventPublisher
{
    private readonly List<INotificationObserver> _observers = new();
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _logger = logger;
    }

    public void Subscribe(INotificationObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            _logger.LogInformation($"Observer '{observer.ObserverName}' subscribed to events");
        }
    }

    public void Unsubscribe(INotificationObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
            _logger.LogInformation($"Observer '{observer.ObserverName}' unsubscribed from events");
        }
    }

    public async Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Publishing event: {@event.GetType().Name}");

        var tasks = new List<Task>();
        foreach (var observer in _observers)
        {
            var task = @event switch
            {
                OrderCreatedEvent orderEvent => observer.OnOrderCreatedAsync(orderEvent, cancellationToken),
                PaymentCompletedEvent paymentEvent => observer.OnPaymentCompletedAsync(paymentEvent, cancellationToken),
                UserRegisteredEvent userEvent => observer.OnUserRegisteredAsync(userEvent, cancellationToken),
                ProductStockChangedEvent stockEvent => observer.OnProductStockChangedAsync(stockEvent, cancellationToken),
                _ => Task.CompletedTask
            };
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        _logger.LogInformation($"All observers notified about {@event.GetType().Name}");
    }
}
```

#### **Observer Interface:**

```csharp
public interface INotificationObserver
{
    string ObserverName { get; }
    Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken);
    Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken);
    Task OnUserRegisteredAsync(UserRegisteredEvent @event, CancellationToken cancellationToken);
    Task OnProductStockChangedAsync(ProductStockChangedEvent @event, CancellationToken cancellationToken);
}
```

#### **Concrete Observers:**

```csharp
// 1. Email Notification Service
public class EmailNotificationService : INotificationObserver
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailNotificationService> _logger;

    public string ObserverName => "EmailNotification";

    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[EMAIL] Sending order confirmation email for order {@event.OrderNumber}");

        var emailContent = $@"
            <h2>Order Confirmation</h2>
            <p>Order Number: {@event.OrderNumber}</p>
            <p>Total Amount: {@event.TotalAmount:C}</p>
            <p>Thank you for your purchase!</p>
        ";

        try
        {
            await _emailService.SendEmailAsync(
                recipientEmail: GetUserEmail(@event.UserId),
                subject: $"Order Confirmation - {@event.OrderNumber}",
                htmlContent: emailContent,
                cancellationToken: cancellationToken
            );
            _logger.LogInformation($"[EMAIL] Successfully sent order confirmation for {@event.OrderNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[EMAIL] Failed to send order confirmation: {ex.Message}");
        }
    }

    public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[EMAIL] Sending payment confirmation for order {@event.OrderId}");

        var emailContent = $@"
            <h2>Payment Confirmed</h2>
            <p>Transaction ID: {@event.TransactionId}</p>
            <p>Amount: {@event.Amount:C}</p>
            <p>Payment Method: {@event.PaymentMethod}</p>
        ";

        await _emailService.SendEmailAsync(
            recipientEmail: GetUserEmail(@event.UserId),
            subject: $"Payment Confirmation - {@event.TransactionId}",
            htmlContent: emailContent,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnUserRegisteredAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[EMAIL] Sending welcome email to {@event.Email}");

        var emailContent = $@"
            <h2>Welcome!</h2>
            <p>Hello {<@event.UserName>},</p>
            <p>Thank you for registering on our platform!</p>
            <p>Your account is ready to use.</p>
        ";

        await _emailService.SendEmailAsync(
            recipientEmail: @event.Email,
            subject: "Welcome to Our Platform",
            htmlContent: emailContent,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnProductStockChangedAsync(ProductStockChangedEvent @event, CancellationToken cancellationToken)
    {
        // Admin notification for low stock
        _logger.LogInformation($"[EMAIL] Notifying admin about stock change for product {@event.ProductId}");
        // Implementation...
        await Task.Delay(100);
    }

    private string GetUserEmail(int userId) => $"user{userId}@example.com";
}

// 2. SMS Notification Service
public class SMSNotificationService : INotificationObserver
{
    private readonly ISMSService _smsService;
    private readonly ILogger<SMSNotificationService> _logger;

    public string ObserverName => "SMSNotification";

    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[SMS] Sending order confirmation SMS for order {@event.OrderNumber}");

        var smsContent = $"Your order {@event.OrderNumber} has been confirmed. Total: {@event.TotalAmount:C}. Thank you!";

        try
        {
            await _smsService.SendSMSAsync(
                phoneNumber: GetUserPhone(@event.UserId),
                message: smsContent,
                cancellationToken: cancellationToken
            );
            _logger.LogInformation($"[SMS] Successfully sent SMS for {@event.OrderNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[SMS] Failed to send SMS: {ex.Message}");
        }
    }

    public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[SMS] Sending payment confirmation SMS");

        var smsContent = $"Payment of {@event.Amount:C} confirmed (ID: {@event.TransactionId})";

        await _smsService.SendSMSAsync(
            phoneNumber: GetUserPhone(@event.UserId),
            message: smsContent,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnUserRegisteredAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[SMS] Sending welcome SMS to user");

        var smsContent = $"Hi {<@event.UserName>}, welcome! Your account is active.";

        await _smsService.SendSMSAsync(
            phoneNumber: GetUserPhone(@event.UserId),
            message: smsContent,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnProductStockChangedAsync(ProductStockChangedEvent @event, CancellationToken cancellationToken)
    {
        // Notify admin if stock is low
        _logger.LogInformation($"[SMS] Notifying admin about stock");
        await Task.Delay(100);
    }

    private string GetUserPhone(int userId) => "+84901234567";
}

// 3. Push Notification Service
public class PushNotificationService : INotificationObserver
{
    private readonly IPushNotificationService _pushService;
    private readonly ILogger<PushNotificationService> _logger;

    public string ObserverName => "PushNotification";

    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[PUSH] Sending order created push notification");

        var notification = new PushNotification
        {
            Title = "Order Confirmed",
            Body = $"Your order {@event.OrderNumber} total: {@event.TotalAmount:C}",
            Data = new { OrderId = @event.OrderId, OrderNumber = @event.OrderNumber }
        };

        await _pushService.SendAsync(
            userId: @event.UserId,
            notification: notification,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[PUSH] Sending payment completed push notification");

        var notification = new PushNotification
        {
            Title = "Payment Successful",
            Body = $"Payment of {@event.Amount:C} completed",
            Data = new { TransactionId = @event.TransactionId }
        };

        await _pushService.SendAsync(
            userId: @event.UserId,
            notification: notification,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnUserRegisteredAsync(UserRegisteredEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[PUSH] Sending welcome push notification");

        var notification = new PushNotification
        {
            Title = "Welcome!",
            Body = "Your account is ready to use"
        };

        await _pushService.SendAsync(
            userId: @event.UserId,
            notification: notification,
            cancellationToken: cancellationToken
        );
    }

    public async Task OnProductStockChangedAsync(ProductStockChangedEvent @event, CancellationToken cancellationToken)
    {
        // Could notify users who have this product in wishlist
        _logger.LogInformation($"[PUSH] Notifying interested users about stock change");
        await Task.Delay(100);
    }
}

// 4. Audit Log Service
public class AuditLogService : INotificationObserver
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ILogger<AuditLogService> _logger;

    public string ObserverName => "AuditLog";

    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[AUDIT] Logging order creation");

        var auditLog = new AuditLog
        {
            EventType = "OrderCreated",
            EntityId = @event.OrderId.ToString(),
            UserId = @event.UserId,
            Details = $"Order {@event.OrderNumber} created with amount {@event.TotalAmount:C}",
            Timestamp = @event.OccurredOn
        };

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);
    }

    // Other event handlers...
}
```

#### **Auto-registration of Observers (Hosted Service):**

```csharp
public class NotificationSubscriber : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<NotificationSubscriber> _logger;

    public NotificationSubscriber(
        IServiceProvider serviceProvider,
        IEventPublisher eventPublisher,
        ILogger<NotificationSubscriber> logger)
    {
        _serviceProvider = serviceProvider;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting notification subscription...");

        using (var scope = _serviceProvider.CreateScope())
        {
            // Get all observer services
            var emailService = scope.ServiceProvider.GetRequiredService<EmailNotificationService>();
            var smsService = scope.ServiceProvider.GetRequiredService<SMSNotificationService>();
            var pushService = scope.ServiceProvider.GetRequiredService<PushNotificationService>();
            var auditService = scope.ServiceProvider.GetRequiredService<AuditLogService>();

            // Subscribe to event publisher
            _eventPublisher.Subscribe(emailService);
            _eventPublisher.Subscribe(smsService);
            _eventPublisher.Subscribe(pushService);
            _eventPublisher.Subscribe(auditService);

            _logger.LogInformation("All notification services subscribed to events");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Notification subscription stopped");
        return Task.CompletedTask;
    }
}
```

#### **Sử Dụng Event Publishing:**

```csharp
// When order is created
var order = new Order { ... };
await _unitOfWork.Orders.AddAsync(order);
await _unitOfWork.SaveChangesAsync();

// Publish event
var event = new OrderCreatedEvent(
    orderId: order.Id,
    userId: order.UserId,
    orderNumber: order.OrderNumber,
    totalAmount: order.TotalAmount
);
await _eventPublisher.PublishAsync(event);

// Result: Tất cả subscribers (Email, SMS, Push, Audit) đều được thông báo
//         Có thể xảy ra song song (asynchronously)
```

### 💡 **Chức Năng Chi Tiết:**

| **Observer**          | **Event**        | **Action**              | **Channel** |
| --------------------- | ---------------- | ----------------------- | ----------- |
| **EmailNotification** | OrderCreated     | Gửi email xác nhận      | Email       |
|                       | PaymentCompleted | Gửi biên lai thanh toán | Email       |
|                       | UserRegistered   | Gửi welcome email       | Email       |
| **SMSNotification**   | OrderCreated     | Gửi SMS xác nhận        | SMS         |
|                       | PaymentCompleted | Gửi SMS biên lai        | SMS         |
| **PushNotification**  | OrderCreated     | Gửi push notification   | App         |
|                       | PaymentCompleted | Gửi push                | App         |
| **AuditLog**          | Mọi event        | Ghi log audit trail     | Database    |

### ✅ **Lợi Ích:**

1. **Loose coupling:** Publishers không biết observers
2. **Easy to add:** Thêm observer mà không sửa publisher
3. **Parallel processing:** Tất cả observers chạy async
4. **Single responsibility:** Mỗi observer xử lý một việc

---

## 2.5 **Factory Pattern (Payment Methods)**

📍 **Vị trí:** mtkpm.Infrastructure/Services/Payments/PaymentFactory

### 🎯 **Mục Đích:**

- Tạo payment method objects dựa trên type
- Không cần switch/if statements ở client code
- Dễ thêm payment methods mới

### 📝 **Cấu Trúc:**

#### **Product Interface:**

```csharp
public interface IPaymentMethod
{
    string MethodName { get; }
    decimal TransactionFee { get; }
    Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken);
    Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken);
}
```

#### **Concrete Products:**

```csharp
// 1. Credit Card Payment
public class CreditCardPaymentMethod : IPaymentMethod
{
    private readonly IPaymentGateway _gateway;
    private readonly ILogger<CreditCardPaymentMethod> _logger;

    public string MethodName => "Credit Card";
    public decimal TransactionFee => 0.025m; // 2.5%

    public CreditCardPaymentMethod(IPaymentGateway gateway, ILogger<CreditCardPaymentMethod> logger)
    {
        _gateway = gateway;
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Processing credit card payment for amount {request.Amount:C}");

        try
        {
            var response = await _gateway.ChargeCardAsync(
                cardToken: request.PaymentToken,
                amount: request.Amount,
                currency: "VND",
                cancellationToken: cancellationToken
            );

            if (response.Success)
            {
                var fee = request.Amount * TransactionFee;
                return new PaymentResponse
                {
                    Success = true,
                    TransactionId = response.TransactionId,
                    Amount = request.Amount,
                    Fee = fee,
                    PaymentMethod = MethodName,
                    Timestamp = DateTime.UtcNow
                };
            }

            return new PaymentResponse
            {
                Success = false,
                ErrorMessage = response.ErrorMessage,
                PaymentMethod = MethodName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Credit card payment failed: {ex.Message}");
            return new PaymentResponse
            {
                Success = false,
                ErrorMessage = ex.Message,
                PaymentMethod = MethodName
            };
        }
    }

    public async Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Refunding {amount:C} for transaction {transactionId}");

        var response = await _gateway.RefundChargeAsync(
            transactionId: transactionId,
            amount: amount,
            cancellationToken: cancellationToken
        );

        return new RefundResponse
        {
            Success = response.Success,
            RefundId = response.RefundId,
            Amount = amount,
            Timestamp = DateTime.UtcNow
        };
    }
}

// 2. Bank Transfer Payment
public class BankTransferPaymentMethod : IPaymentMethod
{
    private readonly IBankingService _banking;
    private readonly ILogger<BankTransferPaymentMethod> _logger;

    public string MethodName => "Bank Transfer";
    public decimal TransactionFee => 0m; // No fee

    public BankTransferPaymentMethod(IBankingService banking, ILogger<BankTransferPaymentMethod> logger)
    {
        _banking = banking;
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Initiating bank transfer for amount {request.Amount:C}");

        var bankingDetails = await _banking.GenerateBankDetailsAsync(
            amount: request.Amount,
            orderId: request.OrderId,
            cancellationToken: cancellationToken
        );

        return new PaymentResponse
        {
            Success = true,
            TransactionId = bankingDetails.ReferenceNumber,
            Amount = request.Amount,
            Fee = 0,
            PaymentMethod = MethodName,
            Timestamp = DateTime.UtcNow,
            AdditionalData = new { bankingDetails.BankName, bankingDetails.AccountNumber, bankingDetails.ExpiryTime }
        };
    }

    public async Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Refunding bank transfer {transactionId}");

        // Bank transfers are manual, so refund needs manual approval
        return new RefundResponse
        {
            Success = true,
            RefundId = Guid.NewGuid().ToString(),
            Amount = amount,
            Timestamp = DateTime.UtcNow,
            Message = "Refund is pending bank review"
        };
    }
}

// 3. PayPal Payment
public class PayPalPaymentMethod : IPaymentMethod
{
    private readonly IPayPalClient _paypal;
    private readonly ILogger<PayPalPaymentMethod> _logger;

    public string MethodName => "PayPal";
    public decimal TransactionFee => 0.034m; // 3.4%

    public PayPalPaymentMethod(IPayPalClient paypal, ILogger<PayPalPaymentMethod> logger)
    {
        _paypal = paypal;
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Processing PayPal payment for {request.Amount:C}");

        var result = await _paypal.CreatePaymentAsync(
            amount: request.Amount,
            currency: "USD",
            returnUrl: request.ReturnUrl,
            cancelUrl: request.CancelUrl,
            cancellationToken: cancellationToken
        );

        if (result.Success)
        {
            var fee = request.Amount * TransactionFee;
            return new PaymentResponse
            {
                Success = true,
                TransactionId = result.PaymentId,
                Amount = request.Amount,
                Fee = fee,
                PaymentMethod = MethodName,
                Timestamp = DateTime.UtcNow,
                RedirectUrl = result.ApprovalUrl // User needs to approve on PayPal
            };
        }

        return new PaymentResponse
        {
            Success = false,
            ErrorMessage = result.ErrorMessage,
            PaymentMethod = MethodName
        };
    }

    public async Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken)
    {
        var result = await _paypal.RefundPaymentAsync(
            paymentId: transactionId,
            amount: amount,
            cancellationToken: cancellationToken
        );

        return new RefundResponse
        {
            Success = result.Success,
            RefundId = result.RefundId,
            Amount = amount,
            Timestamp = DateTime.UtcNow
        };
    }
}

// 4. Cash on Delivery (COD)
public class CODPaymentMethod : IPaymentMethod
{
    private readonly ILogger<CODPaymentMethod> _logger;

    public string MethodName => "Cash on Delivery";
    public decimal TransactionFee => 0.03m; // 3% fee when paid

    public CODPaymentMethod(ILogger<CODPaymentMethod> logger)
    {
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Recording COD order for amount {request.Amount:C}");

        // COD is just recorded, actual payment happens on delivery
        return await Task.FromResult(new PaymentResponse
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString(),
            Amount = request.Amount,
            Fee = 0, // No fee until payment is made
            PaymentMethod = MethodName,
            Timestamp = DateTime.UtcNow,
            Message = "Payment will be collected on delivery"
        });
    }

    public async Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Recording refund for COD order");

        // For COD refunds, just adjust the delivery
        return await Task.FromResult(new RefundResponse
        {
            Success = true,
            RefundId = Guid.NewGuid().ToString(),
            Amount = amount,
            Timestamp = DateTime.UtcNow,
            Message = "Refund will be processed as money-back on delivery"
        });
    }
}

// 5. Apple Pay / Google Pay
public class DigitalWalletPaymentMethod : IPaymentMethod
{
    private readonly IPaymentGateway _gateway;
    private readonly ILogger<DigitalWalletPaymentMethod> _logger;

    public string MethodName => "Digital Wallet (Apple/Google Pay)";
    public decimal TransactionFee => 0.02m; // 2%

    public DigitalWalletPaymentMethod(IPaymentGateway gateway, ILogger<DigitalWalletPaymentMethod> logger)
    {
        _gateway = gateway;
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Processing digital wallet payment for {request.Amount:C}");

        var response = await _gateway.ProcessWalletPaymentAsync(
            walletToken: request.PaymentToken,
            amount: request.Amount,
            cancellationToken: cancellationToken
        );

        if (response.Success)
        {
            var fee = request.Amount * TransactionFee;
            return new PaymentResponse
            {
                Success = true,
                TransactionId = response.TransactionId,
                Amount = request.Amount,
                Fee = fee,
                PaymentMethod = MethodName,
                Timestamp = DateTime.UtcNow
            };
        }

        return new PaymentResponse
        {
            Success = false,
            ErrorMessage = response.ErrorMessage,
            PaymentMethod = MethodName
        };
    }

    public async Task<RefundResponse> RefundAsync(string transactionId, decimal amount, CancellationToken cancellationToken)
    {
        var response = await _gateway.RefundWalletPaymentAsync(
            transactionId: transactionId,
            amount: amount,
            cancellationToken: cancellationToken
        );

        return new RefundResponse
        {
            Success = response.Success,
            RefundId = response.RefundId,
            Amount = amount,
            Timestamp = DateTime.UtcNow
        };
    }
}
```

#### **Factory:**

```csharp
public interface IPaymentFactory
{
    IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType);
    IEnumerable<string> GetAvailablePaymentMethods();
}

public class PaymentFactory : IPaymentFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentFactory> _logger;

    public PaymentFactory(IServiceProvider serviceProvider, ILogger<PaymentFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType)
    {
        _logger.LogInformation($"Creating payment method: {paymentType}");

        return paymentType switch
        {
            PaymentMethodType.CreditCard => _serviceProvider.GetRequiredService<CreditCardPaymentMethod>(),
            PaymentMethodType.BankTransfer => _serviceProvider.GetRequiredService<BankTransferPaymentMethod>(),
            PaymentMethodType.PayPal => _serviceProvider.GetRequiredService<PayPalPaymentMethod>(),
            PaymentMethodType.COD => _serviceProvider.GetRequiredService<CODPaymentMethod>(),
            PaymentMethodType.DigitalWallet => _serviceProvider.GetRequiredService<DigitalWalletPaymentMethod>(),
            _ => throw new NotSupportedException($"Payment method {paymentType} is not supported")
        };
    }

    public IEnumerable<string> GetAvailablePaymentMethods()
    {
        return new[]
        {
            nameof(PaymentMethodType.CreditCard),
            nameof(PaymentMethodType.BankTransfer),
            nameof(PaymentMethodType.PayPal),
            nameof(PaymentMethodType.COD),
            nameof(PaymentMethodType.DigitalWallet)
        };
    }
}
```

#### **Sử Dụng Factory:**

```csharp
// CommandHandler
public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentResponseDto>
{
    private readonly IPaymentFactory _paymentFactory;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<PaymentResponseDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Factory automatically creates the right payment method
        var paymentMethod = _paymentFactory.CreatePaymentMethod(request.PaymentMethod);

        var paymentRequest = new PaymentRequest
        {
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentToken = request.CardToken,
            ReturnUrl = "https://example.com/return",
            CancelUrl = "https://example.com/cancel"
        };

        var response = await paymentMethod.ProcessPaymentAsync(paymentRequest, cancellationToken);

        if (response.Success)
        {
            // Record payment in database
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            order.Status = OrderStatus.PaidPending; // Custom status
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish payment completed event
            await _eventPublisher.PublishAsync(
                new PaymentCompletedEvent(order.Id, order.UserId, response.TransactionId, response.Amount, paymentMethod.MethodName),
                cancellationToken
            );
        }

        return new PaymentResponseDto
        {
            Success = response.Success,
            TransactionId = response.TransactionId,
            Message = response.ErrorMessage ?? "Payment processed successfully"
        };
    }
}
```

### 💡 **Chức Năng Chi Tiết:**

| **Payment Method** | **Tính Phí** | **Xử Lý**    | **Hoàn Tiền** | **Hiệu Lực**  |
| ------------------ | ------------ | ------------ | ------------- | ------------- |
| **Credit Card**    | 2.5%         | Đồng bộ      | Tự động       | Ngay          |
| **Bank Transfer**  | 0%           | Manual       | Manual        | 1-3 ngày      |
| **PayPal**         | 3.4%         | Chuyển hướng | Tự động       | 1 ngày        |
| **COD**            | 3%           | Ghi nhận     | Manual        | Khi nhận hàng |
| **Digital Wallet** | 2%           | Đồng bộ      | Tự động       | Ngay          |

### ✅ **Lợi Ích:**

1. **Encapsulation:** Mỗi payment method chứa logic riêng
2. **Easy to extend:** Thêm payment method ko sửa factory
3. **Centralized creation:** Factory là single point to create
4. **Type-safe:** Enum ensures valid payment types

---

# 📋 **TÓM LƯỢC DESIGN PATTERNS THEO MỨC ĐỘ ẢNH HƯỞNG**

## **🔴 Mức Độ Ảnh Hưởng CAO (Core Architecture - 4 patterns)**

| **Pattern**              | **Vị Trí**            | **Lợi Ích Chính**              |
| ------------------------ | --------------------- | ------------------------------ |
| **Clean Architecture**   | Backend toàn bộ       | 4-layer separation, dễ bảo trì |
| **CQRS**                 | Features with MediatR | Commands/Queries separation    |
| **Domain-Driven Design** | Domain Layer          | Business logic centric         |
| **Feature-Based Folder** | Admin/UI Frontend     | Cohesive feature modules       |

## **🟠 Mức Độ Ảnh Hưởng VỪA (Data & Business Logic - 5 patterns)**

| **Pattern**                   | **Vị Trí**           | **Lợi Ích Chính**                 |
| ----------------------------- | -------------------- | --------------------------------- |
| **Repository + Unit of Work** | Infrastructure Layer | Data access abstraction           |
| **Decorator**                 | Discount System      | Composable discount combinations  |
| **Strategy**                  | Pricing Service      | Pluggable pricing algorithms      |
| **Observer**                  | Event Publishing     | Async notification system         |
| **Factory**                   | Payment Methods      | Dynamic payment provider creation |

## **🟡 Mức Độ Ảnh Hưởng THẤP (Cross-cutting Concerns - 8 patterns)**

| **Pattern**              | **Vị Trí**         | **Lợi Ích Chính**           |
| ------------------------ | ------------------ | --------------------------- |
| **Dependency Injection** | Program.cs         | Service lifetime management |
| **Mapper**               | AutoMapper         | Entity ↔ DTO conversion     |
| **DTO**                  | Common/DTOs        | API contract definition     |
| **Soft Delete**          | SoftDeleteEntity   | Logical deletion            |
| **Middleware**           | Middleware/        | Cross-cutting concerns      |
| **Caching**              | MemoryCacheService | Performance optimization    |
| **Retry**                | HttpClientWrapper  | Fault tolerance             |
| **Pipeline Behavior**    | ValidationBehavior | Automatic validation        |

## 📊 **PHÂN TÍCH WORKLOAD:**

| **Phần**                     | **Số Công Việc** | **Độ Phức Tạp**           | **Thời Gian Ước Tính** |
| ---------------------------- | ---------------- | ------------------------- | ---------------------- |
| **A - Frontend**             | 7 tasks          | ⭐⭐⭐ (Trung bình)       | 2-3 tuần               |
| **B - Backend Architecture** | 7 tasks          | ⭐⭐⭐⭐⭐ (Rất phức tạp) | 3-4 tuần               |
| **C - Business Logic**       | 6 tasks          | ⭐⭐⭐⭐ (Phức tạp)       | 2-3 tuần               |
| **D - Data & Services**      | 7 tasks          | ⭐⭐⭐ (Trung bình)       | 2-3 tuần               |
| **E - Testing & Docs**       | 3 tasks          | ⭐⭐ (Đơn giản)           | 1-2 tuần               |
| **TỔNG CỘNG**                | **30 tasks**     | ⭐⭐⭐⭐                  | **10-15 tuần**         |

---

### ✅ **Điểm Mạnh của Codebase:**

1. **Proper separation of concerns** - Data, business logic, presentation rõ ràng
2. **Event-driven architecture** - Loose coupling through pub/sub
3. **Flexible pricing & discounts** - Strategy & Decorator patterns cho business needs
4. **Comprehensive validation** - Automatic through pipeline behavior
5. **Scalable design** - Can easily extract to microservices

### 🎓 **Áp Dụng cho Môn Học:**

Dự án này là một **exemplary case study** các design patterns vì:

- Sử dụng chúng **một cách thích hợp** (not overused)
- Mỗi pattern **giải quyết một vấn đề cụ thể**
- **Real-world patterns** thường được sử dụng trong industry
- Architecture **scalable & maintainable**
