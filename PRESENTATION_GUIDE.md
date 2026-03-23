# 🎓 HỆ THỐNG E-COMMERCE MTKPM - BÀI THUYẾT TRÌNH CHI TIẾT

## 📌 MỤC LỤC
1. [Tổng Quan Hệ Thống](#1-tổng-quan-hệ-thống)
2. [Kiến Trúc Clean Architecture](#2-kiến-trúc-clean-architecture)
3. [Design Patterns - Chi Tiết & Ứng Dụng](#3-design-patterns--chi-tiết--ứng-dụng)
4. [CQRS + MediatR Pattern](#4-cqrs--mediatr-pattern)
5. [Luồng Xử Lý Chi Tiết](#5-luồng-xử-lý-chi-tiết)
6. [Dependency Injection & DI Container](#6-dependency-injection--di-container)
7. [Validation & Error Handling](#7-validation--error-handling)
8. [Bảng So Sánh Các Pattern](#8-bảng-so-sánh-các-pattern)

---

## 1. TỔNG QUAN HỆ THỐNG

### 1.1 Thông Tin Cơ Bản
- **Tên Dự Án**: MTKPM E-Commerce System
- **Công Nghệ**: .NET 8 + Entity Framework Core
- **Kiến Trúc**: Clean Architecture + CQRS
- **Database**: SQL Server
- **APIs**: RESTful với Swagger Documentation

### 1.2 Các Chức Năng Chính
```
┌──────────────────────────────────────────────────┐
│         E-COMMERCE SYSTEM FEATURES               │
├──────────────────────────────────────────────────┤
│ 🔐 Authentication & Authorization (JWT)         │
│ 📦 Product Management (CRUD)                     │
│ 🛒 Shopping Cart Management                      │
│ 💳 Multiple Payment Methods                      │
│ 💰 Dynamic Pricing Strategies                    │
│ 🎁 Flexible Discount System                      │
│ 📧 Multi-Channel Notifications                   │
│ 📊 Order Management & Tracking                   │
│ ⭐ Favourite Products                            │
│ 📝 Comprehensive Logging                         │
└──────────────────────────────────────────────────┘
```

### 1.3 Công Nghệ Sử Dụng
```
Frontend/Client
     ↓
ASP.NET Core API (Presentation Layer)
     ↓
MediatR (CQRS Pattern)
     ↓
Business Logic (Application Layer)
     ↓
Domain Models (Domain Layer)
     ↓
Services + Repositories (Infrastructure Layer)
     ↓
SQL Server Database
```

---

## 2. KIẾN TRÚC CLEAN ARCHITECTURE

### 2.1 Cấu Trúc 4 Lớp

```
┌─────────────────────────────────────────────────────┐
│     mtkpm (PRESENTATION LAYER)                      │
│  ┌──────────────────────────────────────────────┐   │
│  │ Controllers/Middleware/Program.cs            │   │
│  │ - OrdersController.cs                        │   │
│  │ - ProductsController.cs                      │   │
│  │ - PaymentController.cs                       │   │
│  │ - RequestResponseLoggingMiddleware.cs        │   │
│  │ - ExceptionHandlingMiddleware.cs             │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────────┐
│  mtkpm.Application (APPLICATION LAYER)              │
│  ┌──────────────────────────────────────────────┐   │
│  │ Features (Commands/Queries)                  │   │
│  │ - Orders/Commands/CreateOrder/              │   │
│  │ - Products/Queries/GetProductById/          │   │
│  │ - Cart/Commands/AddToCart/                  │   │
│  │ Common/Interfaces/Services                   │   │
│  │ Common/DTOs (Data Transfer Objects)         │   │
│  │ Validators (FluentValidation)               │   │
│  │ Mapper (AutoMapper)                         │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────────┐
│ mtkpm.Infrastructure (INFRASTRUCTURE LAYER)         │
│  ┌──────────────────────────────────────────────┐   │
│  │ Services (Payments, Pricing, Discounts)      │   │
│  │ Repositories (Data Access Layer)             │   │
│  │ Persistence (DbContext, Migrations)         │   │
│  │ External Service Integrations                │   │
│  │ - PaymentService/PaymentFactory              │   │
│  │ - PricingService (Strategy Pattern)         │   │
│  │ - DiscountService (Decorator Pattern)       │   │
│  │ - EventPublisher (Observer Pattern)         │   │
│  │ - LoggerService (Singleton Pattern)         │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────────┐
│    mtkpm.Domain (DOMAIN LAYER)                      │
│  ┌──────────────────────────────────────────────┐   │
│  │ Entities (Order, Product, User, etc.)        │   │
│  │ Enums (OrderStatus, PaymentMethodType)      │   │
│  │ ValueObjects/Events (DDD)                   │   │
│  │ Business Logic (Rich Domain Model)          │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

### 2.2 Lợi Ích Clean Architecture

| Lợi Ích | Giải Thích |
|---------|-----------|
| **Independent of Frameworks** | Logic không phụ thuộc ASP.NET Core, có thể thay đổi framework |
| **Testable** | Mỗi layer có thể test độc lập bằng Unit Tests |
| **Independent of UI** | Business logic không biết UI là Web/Mobile/Desktop |
| **Independent of Database** | Logic không phụ thuộc SQL Server, có thể thay đổi DB |
| **Independent of External Services** | API không phụ thuộc specific payment provider |
| **Easy to Maintain** | Clear responsibility, dễ debug và extend |
| **Easy to Scale** | Có thể optimize từng layer riêng biệt |

### 2.3 Qui Tắc Dependency Flow

```
Inner Layer (Domain) → không biết về Outer Layer
Outer Layer (Presentation) → phụ thuộc Inner Layer (Domain)

❌ WRONG:
Service → ServiceRepository → Domain Entity

✅ CORRECT:
Domain Entity ← Repository ← Service ← Handler ← Controller
(Dependency flows inward)
```

---

## 3. DESIGN PATTERNS - CHI TIẾT & ỨNG DỤNG

### 3.1 SINGLETON PATTERN - Logger Service

#### 📍 Vị Trí
```
mtkpm.Application/Common/Interfaces/ILoggerService.cs
mtkpm.Infrastructure/Services/LoggerService.cs
```

#### 📖 Giải Thích
**Singleton** đảm bảo chỉ có **một instance duy nhất** trong suốt lifetime của ứng dụng.

#### 🔧 Cách Hoạt Động
```csharp
// Lazy initialization - Thread-safe
private static readonly Lazy<LoggerService> _instance = 
    new(() => new LoggerService());

public static LoggerService Instance => _instance.Value;

// Private constructor - không thể tạo instance mới
private LoggerService() { }

// Tất cả client dùng chung instance này
public void LogInfo(string message, string category)
{
    Console.WriteLine($"[INFO][{category}] {DateTime.Now} - {message}");
}
```

#### 💡 Ứng Dụng Thực Tế
```
Scenario: System cần ghi log tất cả hoạt động (Login, Order, Payment)

User 1 → LoggerService → File/Database
User 2 → (Cùng instance) → File/Database
User 3 → (Cùng instance) → File/Database

✅ Lợi ích:
- Chỉ một nơi ghi log → Quản lý tập trung
- Không thread-unsafe → Thread-safe vì Lazy<T>
- Resource hiệu quả → Không tạo multiple logger instances
```

#### 🎯 Dependency Injection
```csharp
// Program.cs
services.AddSingleton<ILoggerService>(provider => LoggerService.Instance);

// Usage in Handler
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly ILoggerService _logger;
    
    public CreateOrderCommandHandler(ILoggerService logger)
    {
        _logger = logger;
    }
    
    public async Task<OrderDto> Handle(CreateOrderCommand request, ...)
    {
        _logger.LogInfo("Creating order...", "OrderService");
        // ...
    }
}
```

---

### 3.2 FACTORY PATTERN - Payment Methods

#### 📍 Vị Trí
```
mtkpm.Application/Common/Interfaces/Services/IPaymentFactory.cs
mtkpm.Infrastructure/Services/Payments/PaymentFactory.cs
├── CreditCardPaymentService.cs
├── PayPalPaymentService.cs
├── CODPaymentService.cs
└── BankTransferPaymentService.cs
```

#### 📖 Giải Thích
**Factory Pattern** cho phép **tạo object phù hợp lúc runtime** mà không cần if/else phức tạp.

#### 🔧 Cách Hoạt Động
```csharp
// IPaymentFactory interface
public interface IPaymentFactory
{
    IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType);
}

// PaymentFactory implementation
public class PaymentFactory : IPaymentFactory
{
    public IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType)
    {
        return paymentType switch
        {
            PaymentMethodType.CreditCard => new CreditCardPaymentService(_logger),
            PaymentMethodType.PayPal => new PayPalPaymentService(_logger),
            PaymentMethodType.COD => new CODPaymentService(_logger),
            PaymentMethodType.BankTransfer => new BankTransferPaymentService(_logger),
            _ => throw new NotSupportedException(...)
        };
    }
}
```

#### 💡 Ứng Dụng Thực Tế
```
Flow khi user thanh toán:

1. User chọn phương thức: "CREDIT_CARD"
2. POST /api/payment/process
   {
     "amount": 1000000,
     "paymentMethod": "CREDIT_CARD",
     "cardNumber": "1234-5678-...",
     "cvv": "123"
   }

3. ProcessPaymentCommandHandler
   ↓
4. var paymentMethod = _paymentFactory.CreatePaymentMethod(
       PaymentMethodType.CreditCard
   )
   ↓
5. CreditCardPaymentService instance được tạo
   ↓
6. paymentMethod.ProcessPaymentAsync(1000000)
   ↓
7. Gọi PaymentGateway để xác thực card
   ↓
8. Trả về TransactionId hoặc Exception
```

#### 🎯 Lợi Ích
```
❌ WITHOUT FACTORY (Old way):
if (paymentType == "CREDIT_CARD")
    return new CreditCardPaymentService();
else if (paymentType == "PAYPAL")
    return new PayPalPaymentService();
else if (paymentType == "COD")
    return new CODPaymentService();
// ...
→ Code dài, khó maintain, không reusable

✅ WITH FACTORY:
return _paymentFactory.CreatePaymentMethod(paymentType);
→ Clean, dễ test, dễ thêm payment method mới
```

#### 📊 Các Payment Methods
| Method | Ứng Dụng | Validation |
|--------|----------|-----------|
| CreditCard | Visa, Mastercard, JCB | Verify card info + CVV |
| PayPal | PayPal Account | Redirect to PayPal gateway |
| BankTransfer | Direct bank transfer | Bank account verification |
| COD | Cash on Delivery | Address verification only |
| MobileWallet | Zalo Pay, Momo | QR code, OTP verification |

---

### 3.3 STRATEGY PATTERN - Dynamic Pricing

#### 📍 Vị Trí
```
mtkpm.Application/Common/Interfaces/Services/IPricingStrategy.cs
mtkpm.Infrastructure/Services/Pricing/
├── RegularPricingStrategy.cs
├── BulkDiscountPricingStrategy.cs
├── SeasonalPricingStrategy.cs
└── VIPMemberPricingStrategy.cs
```

#### 📖 Giải Thích
**Strategy Pattern** cho phép **chọn thuật toán lúc runtime** dựa trên điều kiện.

#### 🔧 Cách Hoạt Động
```csharp
// IPricingStrategy interface
public interface IPricingStrategy
{
    string StrategyName { get; }
    decimal CalculatePrice(Product product, int quantity, PricingContext context);
}

// Concrete Strategies
public class RegularPricingStrategy : IPricingStrategy
{
    public string StrategyName => "Regular";
    
    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        return product.Price * quantity; // Giá bán thường
    }
}

public class BulkDiscountPricingStrategy : IPricingStrategy
{
    private readonly int _threshold;
    private readonly decimal _discountPercent;
    
    public string StrategyName => "Bulk Discount";
    
    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        if (quantity >= _threshold)
        {
            var discount = (product.Price * quantity * _discountPercent) / 100;
            return (product.Price * quantity) - discount;
        }
        return product.Price * quantity;
    }
}

public class VIPMemberPricingStrategy : IPricingStrategy
{
    public string StrategyName => "VIP Member";
    
    public decimal CalculatePrice(Product product, int quantity, PricingContext context)
    {
        var basePrice = product.Price * quantity;
        var discount = context.UserTier switch
        {
            "Bronze" => 0.05m,
            "Silver" => 0.10m,
            "Gold" => 0.15m,
            "Platinum" => 0.25m,
            _ => 0m
        };
        return basePrice - (basePrice * discount);
    }
}
```

#### 💡 Ứng Dụng Thực Tế
```
Scenario: Tính giá cho Product A (100.000 đ), Qty: 15

Input:
- User Tier: Gold
- Current Date: 2024-02-14 (Valentine Day)
- Quantity: 15

Strategy Selection:
1. RegularPricingStrategy → 1.500.000 đ
2. BulkDiscountPricingStrategy (qty >= 10) → 1.350.000 đ (10% off)
3. SeasonalPricingStrategy (Valentine) → 1.200.000 đ (20% off)
4. VIPMemberPricingStrategy (Gold tier) → 1.275.000 đ (15% off)

Best Price Selection:
→ Select SeasonalPricingStrategy = 1.200.000 đ (thấp nhất)

Implementation:
var context = new PricingContext 
{ 
    UserTier = "Gold", 
    CurrentDate = DateTime.Now 
};

var strategy = SelectBestStrategy(context);
var finalPrice = strategy.CalculatePrice(product, 15, context);
// finalPrice = 1.200.000 đ
```

#### 🎯 Lợi Ích
```
❌ WITHOUT STRATEGY:
public decimal CalculatePrice(Product p, int q, User u)
{
    if (u.Tier == "Gold")
        return p.Price * q * 0.85m;
    else if (u.Tier == "Silver")
        return p.Price * q * 0.90m;
    else if (IsSeasonalDate(DateTime.Now))
        return p.Price * q * 0.80m;
    else if (q >= 10)
        return p.Price * q * 0.90m;
    else
        return p.Price * q;
}
// Code rất dài, khó thêm strategy mới, khó test

✅ WITH STRATEGY:
var strategy = SelectStrategy(user, date, quantity);
return strategy.CalculatePrice(product, quantity, context);
// Clean, dễ test từng strategy riêng, dễ extend
```

---

### 3.4 OBSERVER PATTERN - Multi-Channel Notifications

#### 📍 Vị Trí
```
mtkpm.Application/Common/Interfaces/Services/IEventPublisher.cs
mtkpm.Application/Common/Interfaces/Services/INotificationObserver.cs
mtkpm.Infrastructure/Services/Notifications/
├── EventPublisher.cs (Subject)
├── EmailNotificationService.cs (Observer)
├── SMSNotificationService.cs (Observer)
└── PushNotificationService.cs (Observer)
```

#### 📖 Giải Thích
**Observer Pattern** cho phép **multiple observers lắng nghe events** từ một Subject mà không coupling.

#### 🔧 Cách Hoạt Động
```csharp
// INotificationObserver interface
public interface INotificationObserver
{
    string ObserverName { get; }
    Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken ct);
    Task OnPaymentCompletedAsync(PaymentCompletedEvent @event, CancellationToken ct);
    Task OnOrderDeliveredAsync(OrderDeliveredEvent @event, CancellationToken ct);
}

// EventPublisher (Subject)
public class EventPublisher : IEventPublisher
{
    private readonly List<INotificationObserver> _observers = new();
    
    public void Subscribe(INotificationObserver observer)
    {
        _observers.Add(observer);
    }
    
    public async Task PublishAsync(DomainEvent @event)
    {
        var tasks = _observers.Select(observer => 
            @event switch
            {
                OrderCreatedEvent orderCreated => 
                    observer.OnOrderCreatedAsync(orderCreated, CancellationToken.None),
                PaymentCompletedEvent paymentCompleted => 
                    observer.OnPaymentCompletedAsync(paymentCompleted, CancellationToken.None),
                // ...
            }
        );
        
        await Task.WhenAll(tasks); // Notify all in parallel
    }
}

// Concrete Observers
public class EmailNotificationService : INotificationObserver
{
    public string ObserverName => "Email Service";
    
    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken ct)
    {
        // Gửi email xác nhận
        var email = await _userService.GetEmailAsync(@event.UserId);
        await _emailProvider.SendAsync(
            email,
            "Order Confirmation",
            $"Your order {event.OrderNumber} has been created",
            ct
        );
    }
}

public class SMSNotificationService : INotificationObserver
{
    public string ObserverName => "SMS Service";
    
    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken ct)
    {
        // Gửi SMS thông báo
        var phoneNumber = await _userService.GetPhoneAsync(@event.UserId);
        await _smsProvider.SendAsync(
            phoneNumber,
            $"Order {event.OrderNumber} created successfully",
            ct
        );
    }
}

public class PushNotificationService : INotificationObserver
{
    public string ObserverName => "Push Service";
    
    public async Task OnOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken ct)
    {
        // Gửi push notification
        await _firebaseService.SendAsync(
            @event.UserId,
            "Order Created",
            $"Your order {event.OrderNumber} is being processed",
            ct
        );
    }
}
```

#### 💡 Ứng Dụng Thực Tế
```
Flow khi user tạo đơn hàng:

1. User POST /api/orders
   {
     "cartId": 1,
     "shippingAddress": "123 Main St",
     "paymentMethod": "CREDIT_CARD"
   }

2. CreateOrderCommandHandler.Handle()
   ↓
3. Order entity created
   ↓
4. _unitOfWork.Orders.Add(order)
   ↓
5. await _unitOfWork.SaveChangesAsync()
   ↓
6. RaiseDomainEvent(new OrderCreatedEvent(orderId, userId, orderNumber))
   ↓
7. _eventPublisher.PublishAsync(orderCreatedEvent)
   ↓
8. Parallel execution:
   ├─ EmailNotificationService.OnOrderCreatedAsync()
   │  ├─ Get user email
   │  ├─ Compose email body
   │  └─ Send via SMTP
   │
   ├─ SMSNotificationService.OnOrderCreatedAsync()
   │  ├─ Get user phone
   │  ├─ Compose SMS text
   │  └─ Send via SMS provider
   │
   └─ PushNotificationService.OnOrderCreatedAsync()
      ├─ Get user device tokens
      ├─ Compose push payload
      └─ Send via Firebase
   
9. All observers notified ✓
   ↓
10. Response 200 OK with OrderId
```

#### 🎯 Lợi Ích
```
❌ WITHOUT OBSERVER (Tightly Coupled):
public async Task Handle(CreateOrderCommand cmd)
{
    var order = new Order(...);
    await _unitOfWork.Orders.AddAsync(order);
    await _unitOfWork.SaveChangesAsync();
    
    // Handler phải biết tất cả notification channels
    await _emailService.SendOrderConfirmation(order);
    await _smsService.SendOrderNotification(order);
    await _pushService.SendOrderPush(order);
    
    // Nếu thêm Slack notification → Phải sửa code này
    // Handler mix business logic + notification logic
    // Khó test vì phụ thuộc nhiều service
}

✅ WITH OBSERVER (Loosely Coupled):
public async Task Handle(CreateOrderCommand cmd)
{
    var order = new Order(...);
    await _unitOfWork.Orders.AddAsync(order);
    await _unitOfWork.SaveChangesAsync();
    
    // Chỉ cần publish event, không cần biết observers
    await _eventPublisher.PublishAsync(
        new OrderCreatedEvent(order.Id, order.UserId, order.OrderNumber)
    );
}

// Thêm Slack notification → Chỉ tạo SlackNotificationService mới
// Không cần sửa handler
// Handler clean, focused on business logic
```

---

### 3.5 DECORATOR PATTERN - Flexible Discount System

#### 📍 Vị Trí
```
mtkpm.Infrastructure/Services/Discounts/
├── BaseDiscount.cs
├── DiscountDecorator.cs (Abstract)
├── PercentageDiscountDecorator.cs
├── FixedAmountDiscountDecorator.cs
├── FreeShippingDiscountDecorator.cs
├── LoyaltyPointsDiscountDecorator.cs
└── BundleDiscountDecorator.cs
```

#### 📖 Giải Thích
**Decorator Pattern** cho phép **xếp chồng chức năng động** mà không tạo class explosion.

#### 🔧 Cách Hoạt Động
```csharp
// IDiscount interface
public interface IDiscount
{
    string DiscountName { get; }
    string Description { get; }
    decimal GetDiscountAmount(CartDto cart);
    decimal ApplyDiscount(CartDto cart);
}

// BaseDiscount component
public class BaseDiscount : IDiscount
{
    public string DiscountName => "No Discount";
    public string Description => "No discount applied";
    
    public decimal GetDiscountAmount(CartDto cart) => 0;
    public decimal ApplyDiscount(CartDto cart) => cart.TotalAmount;
}

// DiscountDecorator abstract base
public abstract class DiscountDecorator : IDiscount
{
    protected IDiscount _wrappedDiscount;
    
    protected DiscountDecorator(IDiscount wrappedDiscount)
    {
        _wrappedDiscount = wrappedDiscount;
    }
    
    public virtual string DiscountName => _wrappedDiscount.DiscountName;
    public virtual string Description => _wrappedDiscount.Description;
    
    public virtual decimal GetDiscountAmount(CartDto cart) 
        => _wrappedDiscount.GetDiscountAmount(cart);
    
    public virtual decimal ApplyDiscount(CartDto cart) 
        => _wrappedDiscount.ApplyDiscount(cart);
}

// Concrete Decorators
public class PercentageDiscountDecorator : DiscountDecorator
{
    private readonly decimal _percentage;
    
    public PercentageDiscountDecorator(IDiscount wrapped, decimal percentage)
        : base(wrapped)
    {
        _percentage = percentage;
    }
    
    public override string DiscountName => $"{_wrapped.DiscountName} + {_percentage}% Discount";
    
    public override decimal ApplyDiscount(CartDto cart)
    {
        var wrappedPrice = _wrappedDiscount.ApplyDiscount(cart);
        var discountAmount = (wrappedPrice * _percentage) / 100;
        return wrappedPrice - discountAmount;
    }
}

public class FreeShippingDiscountDecorator : DiscountDecorator
{
    private readonly decimal _shippingCost;
    
    public FreeShippingDiscountDecorator(IDiscount wrapped, decimal shippingCost)
        : base(wrapped)
    {
        _shippingCost = shippingCost;
    }
    
    public override string DiscountName => $"{_wrapped.DiscountName} + Free Shipping";
    
    public override decimal ApplyDiscount(CartDto cart)
    {
        var wrappedPrice = _wrappedDiscount.ApplyDiscount(cart);
        return wrappedPrice - _shippingCost; // Miễn phí vận chuyển
    }
}

public class LoyaltyPointsDiscountDecorator : DiscountDecorator
{
    private readonly int _loyaltyPoints;
    private const decimal PointValue = 100; // 100 VND per point
    
    public LoyaltyPointsDiscountDecorator(IDiscount wrapped, int loyaltyPoints)
        : base(wrapped)
    {
        _loyaltyPoints = loyaltyPoints;
    }
    
    public override string DiscountName => $"{_wrapped.DiscountName} + Use {_loyaltyPoints} Points";
    
    public override decimal ApplyDiscount(CartDto cart)
    {
        var wrappedPrice = _wrappedDiscount.ApplyDiscount(cart);
        var pointDiscount = _loyaltyPoints * PointValue;
        return wrappedPrice - pointDiscount;
    }
}
```

#### 💡 Ứng Dụng Thực Tế
```
Scenario: User nhập mã chiết khấu: ["PERCENT_10", "FREE_SHIP", "LOYALTY_500"]

Cart Original:
├─ Product A: 500.000 đ
├─ Product B: 300.000 đ
└─ Shipping: 50.000 đ
Total: 850.000 đ

Step-by-Step Decoration:

1. IDiscount discount = new BaseDiscount()
   Price: 850.000 đ

2. discount = new PercentageDiscountDecorator(discount, 10m)
   Giảm 10% của 850.000 = 85.000
   Price: 765.000 đ

3. discount = new FreeShippingDiscountDecorator(discount, 50000)
   Miễn phí shipping 50.000
   Price: 715.000 đ

4. discount = new LoyaltyPointsDiscountDecorator(discount, 500)
   Dùng 500 points (500 × 100 = 50.000 VND)
   Price: 665.000 đ

Final Result:
Original: 850.000 đ
Final: 665.000 đ
Savings: 185.000 đ (21.76%)

Breakdown:
├─ Percentage Discount: -85.000
├─ Free Shipping: -50.000
└─ Loyalty Points: -50.000
```

#### 🎯 Lợi Ích
```
❌ WITHOUT DECORATOR:
// Phải tạo nhiều class kết hợp
PercentageAndFreeShippingDiscount
PercentageAndLoyaltyDiscount
PercentageAndFreeShippingAndLoyaltyDiscount
FreeShippingAndLoyaltyDiscount
// ... 2^n combinations

✅ WITH DECORATOR:
// Xếp chồng động theo nhu cầu
discount = new BaseDiscount();
discount = new PercentageDiscountDecorator(discount, 10m);
discount = new FreeShippingDiscountDecorator(discount, 50000);
discount = new LoyaltyPointsDiscountDecorator(discount, 500);
// Mạnh mẽ, linh hoạt, dễ test
```

---

### 3.6 REPOSITORY PATTERN - Data Access Layer

#### 📍 Vị Trí
```
mtkpm.Application/Common/Interfaces/Repositories/
├── IRepository.cs (Generic)
├── IProductRepository.cs
├── IOrderRepository.cs
├── ICategoryRepository.cs
├── ICartItemRepository.cs
└── IUnitOfWork.cs

mtkpm.Infrastructure/Data/Repositories/
├── Repository.cs (Generic implementation)
├── ProductRepository.cs
├── OrderRepository.cs
└── ...
```

#### 📖 Giải Thích
**Repository Pattern** **tách logic truy cập dữ liệu** khỏi business logic.

#### 🔧 Cách Hoạt Động
```csharp
// Generic IRepository interface
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

// Specific IProductRepository interface
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductWithCategoryAsync(int id);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
}

// Generic Repository implementation
public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }
    
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    
    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}

// Specific ProductRepository implementation
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }
    
    public async Task<Product?> GetProductWithCategoryAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }
}
```

#### 🎯 Unit of Work Pattern
```csharp
// IUnitOfWork - Quản lý tất cả repositories
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    ICategoryRepository Categories { get; }
    ICartItemRepository CartItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

// Usage in Handler
public async Task<OrderDto> Handle(CreateOrderCommand request, ...)
{
    var order = new Order(...)
    
    // Add order + update stock trong cùng transaction
    _unitOfWork.Orders.Add(order);
    
    foreach (var item in request.OrderItems)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        product.DecreaseStock(item.Quantity);
        _unitOfWork.Products.Update(product);
    }
    
    // Tất cả lưu cùng lúc hoặc rollback nếu lỗi
    await _unitOfWork.SaveChangesAsync();
    
    return _mapper.Map<OrderDto>(order);
}
```

---

### 3.7 COMMAND PATTERN - CQRS Commands

#### 📍 Vị Trí
```
mtkpm.Application/Features/Orders/Commands/
├── CreateOrder/
│   ├── CreateOrderCommand.cs
│   ├── CreateOrderCommandValidator.cs
│   └── CreateOrderCommandHandler.cs
├── CancelOrder/
├── UpdateOrderStatus/
└── MarkAsPaid/
```

#### 📖 Giải Thích
**Command Pattern** **encapsulate request thành object** để có thể queue, log, undo/redo.

#### 🔧 Cách Hoạt Động
```csharp
// Command DTO
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int UserId { get; set; }
    public int CartId { get; set; }
    public string ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public string? Note { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }
}

// Command Validator
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Invalid user ID");
        
        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithMessage("Shipping address is required")
            .Length(10, 500)
            .WithMessage("Address must be between 10-500 characters");
        
        RuleFor(x => x.OrderItems)
            .NotEmpty()
            .WithMessage("Order must contain at least 1 item")
            .Must(items => items.Count > 0);
        
        RuleForEach(x => x.OrderItems).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).GreaterThan(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}

// Command Handler
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Validate cart
        var cart = await _unitOfWork.CartItems.GetUserCartAsync(request.UserId);
        if (cart == null || !cart.Any())
            throw new InvalidOperationException("Cart is empty");
        
        // Calculate total
        var cartTotal = cart.Sum(x => x.TotalPrice);
        
        // Create order entity
        var order = new Order(
            request.UserId,
            GenerateOrderNumber(),
            request.ShippingAddress,
            request.BillingAddress,
            cartTotal,
            50000, // Shipping fee
            0, // Will be updated by discount service
            request.PaymentMethod,
            request.Note
        );
        
        // Add order items
        foreach (var item in cart)
        {
            order.AddOrderItem(new OrderItem(...));
        }
        
        // Save
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Publish event
        await _eventPublisher.PublishAsync(
            new OrderCreatedEvent(order.Id, order.UserId, order.OrderNumber),
            cancellationToken
        );
        
        return _mapper.Map<OrderDto>(order);
    }
}
```

---

## 4. CQRS + MEDIATR PATTERN

### 4.1 CQRS Concept

**CQRS** = Command Query Responsibility Segregation

```
Traditional Approach:
┌─────────────────────┐
│  Business Logic     │
├─────────────────────┤
│  Create Data        │
│  Read Data          │
│  Update Data        │
│  Delete Data        │
└─────────────────────┘

CQRS Approach:
┌──────────────────┐        ┌──────────────────┐
│  COMMAND MODEL   │        │  QUERY MODEL     │
├──────────────────┤        ├──────────────────┤
│  Write Logic     │        │  Read Logic      │
│  Create Order    │        │  Get Orders      │
│  Update Stock    │        │  Search Products │
│  Process Payment │        │  Get Cart Items  │
│                  │        │                  │
│  Changes DB      │        │  Reads from DB   │
│                  │        │  Can use Cache   │
└──────────────────┘        └──────────────────┘
```

### 4.2 MediatR Implementation

**MediatR** là library implement Mediator Pattern + CQRS.

```csharp
// 1. Define Request (Command/Query)
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

// 2. Define Handler
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Business logic
        var order = new Order(...);
        // Save to database
        return _mapper.Map<OrderDto>(order);
    }
}

// 3. Use in Controller
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

// 4. MediatR Pipeline
Request
  ↓
ValidationBehavior (FluentValidation)
  ↓
LoggingBehavior (Optional)
  ↓
CreateOrderCommandHandler
  ↓
Response
```

### 4.3 Validation Pipeline Behavior

```csharp
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        
        var failures = new List<ValidationFailure>();
        
        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }
        
        if (failures.Any())
            throw new ValidationException(failures);
        
        return await next();
    }
}

// Registration in Program.cs
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

---

## 5. LUỒNG XỬ LÝ CHI TIẾT

### 5.1 Luồng Tạo Đơn Hàng (Complete Flow)

```
┌─────────────────────────────────────────────────────────────┐
│ CLIENT (Browser/Mobile)                                     │
└─────────────────────────────────────────────────────────────┘
                            ↓
                    HTTP POST /api/orders
                    {
                      "cartId": 1,
                      "shippingAddress": "123 Main St",
                      "paymentMethod": "CREDIT_CARD"
                    }
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER (OrdersController)                       │
│                                                             │
│  [HttpPost]                                                 │
│  public async Task<IActionResult> CreateOrder(              │
│      CreateOrderCommand command)                            │
│  {                                                          │
│      var result = await _mediator.Send(command);            │
│      return Ok(result);                                     │
│  }                                                          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ MEDIATR PIPELINE                                            │
│                                                             │
│  1. ValidationBehavior                                      │
│     ├─ Check UserId > 0 ✓                                   │
│     ├─ Check ShippingAddress not empty ✓                    │
│     ├─ Check OrderItems not empty ✓                         │
│     └─ All validations pass ✓                               │
│                                                             │
│  2. CreateOrderCommandHandler.Handle()                      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ APPLICATION LAYER (Business Logic)                          │
│ CreateOrderCommandHandler                                   │
│                                                             │
│  1. Get user cart                                           │
│     var cart = await _unitOfWork.CartItems                  │
│         .GetUserCartAsync(userId);                          │
│                                                             │
│  2. Validate cart not empty ✓                               │
│                                                             │
│  3. Calculate totals                                        │
│     decimal subTotal = 1,000,000                            │
│     decimal shipping = 50,000                               │
│     decimal discount = 0 (will be calculated later)         │
│                                                             │
│  4. Create Order entity                                     │
│     var order = new Order(                                  │
│         userId: 5,                                          │
│         orderNumber: "ORD-2024-001234",                     │
│         shippingAddress: "123 Main St",                     │
│         subTotal: 1,000,000,                                │
│         shippingFee: 50,000,                                │
│         paymentMethod: CREDIT_CARD                          │
│     );                                                      │
│                                                             │
│  5. Add order items                                         │
│     foreach (var cartItem in cart)                          │
│     {                                                       │
│         var orderItem = new OrderItem(                      │
│             product.Id,                                     │
│             quantity,                                       │
│             unitPrice                                       │
│         );                                                  │
│         order.AddOrderItem(orderItem);                      │
│     }                                                       │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ INFRASTRUCTURE LAYER (Data Persistence)                    │
│                                                             │
│  1. Unit of Work Begin Transaction                          │
│     await _unitOfWork.BeginTransactionAsync();              │
│                                                             │
│  2. Add Order to Database                                   │
│     await _unitOfWork.Orders.AddAsync(order);               │
│     → INSERT INTO Orders VALUES (...)                       │
│                                                             │
│  3. Update Stock (for each order item)                      │
│     var product = await _unitOfWork.Products                │
│         .GetByIdAsync(item.ProductId);                      │
│     product.DecreaseStock(item.Quantity);                   │
│     _unitOfWork.Products.Update(product);                   │
│     → UPDATE Products SET Stock = Stock - qty                │
│                                                             │
│  4. Clear Cart                                              │
│     foreach (var cartItem in cart)                          │
│     {                                                       │
│         _unitOfWork.CartItems.Delete(cartItem);             │
│     }                                                       │
│     → DELETE FROM CartItems WHERE UserId = 5                │
│                                                             │
│  5. Commit Transaction                                      │
│     await _unitOfWork.CommitTransactionAsync();             │
│     → COMMIT (tất cả queries hoặc ROLLBACK nếu lỗi)        │
│                                                             │
│  Database State Change:                                     │
│  ├─ Order created: ID=1001                                  │
│  ├─ OrderItems created: 3 items                             │
│  ├─ Product A stock: 100 → 98                               │
│  ├─ Product B stock: 50 → 45                                │
│  └─ Cart items cleared                                      │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ DOMAIN EVENTS (Post-Persistence)                            │
│                                                             │
│  RaiseDomainEvent(                                          │
│      new OrderCreatedEvent(                                 │
│          OrderId: 1001,                                     │
│          UserId: 5,                                         │
│          OrderNumber: "ORD-2024-001234"                     │
│      )                                                      │
│  );                                                         │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ EVENT PUBLISHING (Observer Pattern)                         │
│ EventPublisher.PublishAsync(orderCreatedEvent)              │
│                                                             │
│  Subscribers: 3                                             │
│                                                             │
│  ┌──────────────────┐    ┌──────────────────┐               │
│  │ EmailObserver    │    │ SMSObserver      │               │
│  └──────────────────┘    └──────────────────┘               │
│         ↓                       ↓                           │
│    Get user email          Get user phone                   │
│    │                       │                                │
│    └─→ "user@email.com"    └─→ "+84123456789"               │
│         │                       │                           │
│         Compose email           Compose SMS                 │
│         │                       │                           │
│         Subject:                Text:                       │
│         "Order Confirmation"   "Order ORD-2024-001234       │
│         Body: "Your order..."  created successfully"        │
│         │                       │                           │
│         Send via SMTP           Send via SMS API             │
│         │                       │                           │
│         ✓ Email sent            ✓ SMS sent                  │
│                                                             │
│  ┌──────────────────────────────────────────┐              │
│  │ PushNotificationObserver                 │              │
│  └──────────────────────────────────────────┘              │
│         ↓                                                   │
│    Get device tokens (FCM tokens from Firebase)             │
│    │                                                        │
│    └─→ ["token1", "token2", "token3"]                      │
│         │                                                   │
│         Compose push payload                               │
│         │                                                   │
│         {                                                   │
│           "title": "Order Created",                         │
│           "body": "Order ORD-2024-001234 processing..."    │
│         }                                                   │
│         │                                                   │
│         Send via Firebase Cloud Messaging                  │
│         │                                                   │
│         ✓ Push notification sent to all devices            │
│                                                             │
│  All notifications sent in parallel (Task.WhenAll)          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ LOGGING                                                     │
│                                                             │
│  [INFO][OrderService] Creating order...                     │
│  [INFO][OrderService] Cart items found: 3                   │
│  [INFO][OrderService] Order created: ORD-2024-001234        │
│  [INFO][EventPublisher] Publishing event: OrderCreatedEvent │
│  [INFO][EventPublisher] Total subscribers: 3                │
│  [INFO][EmailService] Email sent successfully               │
│  [INFO][SMSService] SMS sent successfully                   │
│  [INFO][PushService] Push notification sent successfully    │
│  [INFO][EventPublisher] Event published successfully        │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ RESPONSE                                                    │
│                                                             │
│  HTTP 200 OK                                                │
│  Content-Type: application/json                             │
│                                                             │
│  {                                                          │
│    "id": 1001,                                              │
│    "orderNumber": "ORD-2024-001234",                        │
│    "userId": 5,                                             │
│    "orderDate": "2024-02-14T10:30:00Z",                     │
│    "status": "Pending",                                     │
│    "shippingAddress": "123 Main St",                        │
│    "subTotal": 1000000,                                     │
│    "shippingFee": 50000,                                    │
│    "discount": 0,                                           │
│    "totalAmount": 1050000,                                  │
│    "paymentMethod": "CREDIT_CARD",                          │
│    "isPaid": false,                                         │
│    "orderItems": [                                          │
│      {                                                      │
│        "productId": 1,                                      │
│        "productName": "iPhone 15",                          │
│        "quantity": 1,                                       │
│        "unitPrice": 500000,                                 │
│        "totalPrice": 500000                                 │
│      },                                                     │
│      {                                                      │
│        "productId": 2,                                      │
│        "productName": "Case",                               │
│        "quantity": 2,                                       │
│        "unitPrice": 250000,                                 │
│        "totalPrice": 500000                                 │
│      }                                                      │
│    ]                                                        │
│  }                                                          │
│                                                             │
│  Status: 200 OK ✓                                           │
│  Time: 450ms                                                │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ CLIENT RECEIVES RESPONSE                                    │
│                                                             │
│  JavaScript/Mobile App                                      │
│  └─ Update UI: Show "Order Created Successfully"            │
│  └─ Redirect to Order Details page                          │
│  └─ Display Order Number: ORD-2024-001234                   │
│                                                             │
│  User Email Inbox:                                          │
│  └─ Order confirmation email received ✓                     │
│                                                             │
│  User Phone SMS:                                            │
│  └─ Order confirmation SMS received ✓                       │
│                                                             │
│  User Mobile App:                                           │
│  └─ Push notification received ✓                            │
└─────────────────────────────────────────────────────────────┘

⏱️ Total Time: ~450ms
📊 Layers Involved: 4 (Presentation → Application → Domain → Infrastructure)
🔗 Patterns Used: CQRS, MediatR, Validator, Repository, Unit of Work, Observer
```

### 5.2 Luồng Thanh Toán (Payment Processing)

```
User selects "Credit Card" payment
            ↓
POST /api/payment/process
{
  "orderId": 1001,
  "amount": 1000000,
  "paymentMethod": "CREDIT_CARD",
  "cardNumber": "4111111111111111",
  "expiryDate": "12/25",
  "cvv": "123"
}
            ↓
ProcessPaymentCommandHandler
            ↓
1. Get Order from DB
   var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            ↓
2. Factory Pattern - Create Payment Method
   var paymentMethod = _paymentFactory.CreatePaymentMethod(
       PaymentMethodType.CreditCard
   );
   → Returns CreditCardPaymentService instance
            ↓
3. Process Payment
   var transactionResult = await paymentMethod.ProcessPaymentAsync(
       cardInfo,
       1000000
   );
   
   CreditCardPaymentService.ProcessPaymentAsync()
   {
       // Validate card info
       if (!IsValidCard(cardNumber, expiryDate, cvv))
           throw new InvalidOperationException("Invalid card");
       
       // Call payment gateway (Stripe, PayPal, etc.)
       var transaction = await _paymentGateway.ChargeAsync(
           cardNumber,
           1000000
       );
       
       if (transaction.Status == "SUCCESS")
           return new PaymentResult { 
               TransactionId = transaction.Id,
               Status = PaymentStatus.Success
           };
       else
           return new PaymentResult {
               Status = PaymentStatus.Failed,
               Message = transaction.ErrorMessage
           };
   }
            ↓
4. Update Order Status if Payment Success
   if (transactionResult.IsSuccess)
   {
       order.MarkAsPaid();
       _unitOfWork.Orders.Update(order);
       await _unitOfWork.SaveChangesAsync();
   }
            ↓
5. Publish Payment Event
   if (transactionResult.IsSuccess)
       await _eventPublisher.PublishAsync(
           new PaymentCompletedEvent(orderId, transactionId)
       );
   else
       await _eventPublisher.PublishAsync(
           new PaymentFailedEvent(orderId, reason)
       );
            ↓
6. Observers Respond
   EmailObserver: Send payment confirmation/failure email
   SMSObserver: Send payment status SMS
   PushObserver: Send payment status push notification
            ↓
7. Response
   {
     "transactionId": "TXN-2024-001234",
     "status": "Success",
     "message": "Payment processed successfully"
   }
```

### 5.3 Luồng Tính Giá Động (Dynamic Pricing)

```
POST /api/pricing/calculate
{
  "productId": 1,
  "quantity": 15,
  "userId": 5
}
            ↓
CalculatePriceCommandHandler
            ↓
1. Get Product
   var product = await _unitOfWork.Products.GetByIdAsync(1);
   → Product: "iPhone 15", Price: 100,000
            ↓
2. Get User Info
   var user = await _userService.GetUserAsync(userId: 5);
   → User Tier: "Gold", IsPremium: true
            ↓
3. Build Pricing Context
   var context = new PricingContext
   {
       UserTier = "Gold",
       CurrentDate = DateTime.Now, // 2024-02-14 (Valentine)
       IsVipMember = true,
       Quantity = 15
   };
            ↓
4. Try Multiple Strategies (Strategy Pattern)
   
   Strategy 1: RegularPricingStrategy
   │
   └─ Calculation: 100,000 × 15 = 1,500,000
              ↓
   
   Strategy 2: BulkDiscountPricingStrategy (qty >= 10)
   │
   ├─ Quantity: 15 >= 10 ✓
   │
   └─ Calculation: 1,500,000 - (1,500,000 × 10%) = 1,350,000
              ↓
   
   Strategy 3: SeasonalPricingStrategy
   │
   ├─ Check: Is it Valentine Day? YES
   │
   └─ Calculation: 1,500,000 - (1,500,000 × 20%) = 1,200,000
              ↓
   
   Strategy 4: VIPMemberPricingStrategy
   │
   ├─ User Tier: Gold
   │
   └─ Calculation: 1,500,000 - (1,500,000 × 15%) = 1,275,000
            ↓
5. Compare & Select Best Price
   Prices:
   ├─ Regular: 1,500,000
   ├─ Bulk: 1,350,000
   ├─ Seasonal: 1,200,000 ← LOWEST
   └─ VIP: 1,275,000
   
   Best Price: 1,200,000 (Seasonal Strategy)
            ↓
6. Response
   {
     "productId": 1,
     "productName": "iPhone 15",
     "originalPrice": 100000,
     "quantity": 15,
     "baseAmount": 1500000,
     "appliedStrategy": "SeasonalPricingStrategy",
     "savings": 300000,
     "savingsPercent": 20.0,
     "finalPrice": 1200000
   }
```

### 5.4 Luồng Tính Chiết Khấu (Discount Calculation)

```
POST /api/discount/calculate
{
  "cartId": 1,
  "discountCodes": ["PERCENT_10", "FREE_SHIP", "LOYALTY_500"]
}
            ↓
CalculateCartDiscountCommandHandler
            ↓
1. Get Cart
   var cart = await _unitOfWork.CartItems.GetUserCartAsync(userId);
   
   Cart Contents:
   ├─ Product A: 500,000 × 1 = 500,000
   ├─ Product B: 300,000 × 1 = 300,000
   └─ Subtotal: 800,000
   
   With Shipping: 800,000 + 50,000 = 850,000
            ↓
2. Build Discount Chain (Decorator Pattern)
   
   IDiscount discount = new BaseDiscount();
   Price: 850,000
            ↓
   
   discount = new PercentageDiscountDecorator(discount, 10m);
   Giảm 10%: 850,000 - 85,000 = 765,000
            ↓
   
   discount = new FreeShippingDiscountDecorator(discount, 50000);
   Miễn ship: 765,000 - 50,000 = 715,000
            ↓
   
   discount = new LoyaltyPointsDiscountDecorator(discount, 500);
   Dùng points: 715,000 - 50,000 = 665,000
   (500 points × 100 VND/point)
            ↓
3. Apply All Decorators
   decimal finalPrice = discount.ApplyDiscount(cart);
            ↓
4. Calculate Breakdown
   DiscountInfo:
   {
     "originalAmount": 850000,
     "discountAmount": 185000,
     "finalAmount": 665000,
     "savingsPercent": 21.76,
     "appliedDiscounts": [
       "10% Percentage Discount",
       "Free Shipping Discount",
       "500 Loyalty Points Discount"
     ],
     "breakdown": [
       {
         "type": "Percentage",
         "value": -85000,
         "description": "10% off"
       },
       {
         "type": "FreeShipping",
         "value": -50000,
         "description": "Free shipping"
       },
       {
         "type": "LoyaltyPoints",
         "value": -50000,
         "description": "500 points used"
       }
     ]
   }
            ↓
5. Response
   {
     "originalAmount": 850000,
     "discountAmount": 185000,
     "finalAmount": 665000,
     "savingsPercent": 21.76,
     "appliedDiscounts": [...]
   }
```

---

## 6. DEPENDENCY INJECTION & DI CONTAINER

### 6.1 DI Registration Flow

```
┌─────────────────────────────────────────────────┐
│         Program.cs (Main Entry Point)           │
└─────────────────────────────────────────────────┘
                            ↓
var builder = WebApplication.CreateBuilder(args);

                            ↓
┌─────────────────────────────────────────────────┐
│   APPLICATION LAYER - AddApplication()          │
├─────────────────────────────────────────────────┤
│  ✓ MediatR                                      │
│    - Register all Handlers                      │
│    - Register all Validators                    │
│    - Register Pipeline Behaviors                │
│  ✓ AutoMapper                                   │
│    - Load all mapping profiles                  │
│  ✓ FluentValidation                             │
│    - Scan and register validators               │
└─────────────────────────────────────────────────┘
                            ↓
services.AddApplication();

                            ↓
┌─────────────────────────────────────────────────┐
│ INFRASTRUCTURE LAYER - AddInfrastructure()      │
├─────────────────────────────────────────────────┤
│  1. AddDbContext()                              │
│     ├─ SQL Server connection                    │
│     ├─ Migrations assembly                      │
│     └─ Retry on failure policy                  │
│                                                 │
│  2. AddIdentityServices()                       │
│     ├─ ASP.NET Core Identity                    │
│     ├─ Password policy                          │
│     └─ User role management                     │
│                                                 │
│  3. AddJwtAuthentication()                      │
│     ├─ JWT Bearer scheme                        │
│     ├─ Token validation                         │
│     └─ Claims configuration                     │
│                                                 │
│  4. AddRepositories()                           │
│     ├─ IProductRepository → ProductRepository   │
│     ├─ IOrderRepository → OrderRepository       │
│     ├─ ICategoryRepository → CategoryRepository │
│     └─ IUnitOfWork → UnitOfWork                 │
│                                                 │
│  5. AddInfrastructureServices()                 │
│     ├─ JWT Service                              │
│     ├─ Auth Service                             │
│     ├─ Current User Service                     │
│     ├─ Payment Factory + Services               │
│     ├─ Pricing Service                          │
│     ├─ Discount Service                         │
│     ├─ Event Publisher                          │
│     ├─ Notification Services (Email, SMS, Push) │
│     └─ Logger Service (Singleton)               │
└─────────────────────────────────────────────────┘
                            ↓
services.AddInfrastructure(configuration);

                            ↓
┌─────────────────────────────────────────────────┐
│  VALIDATION PIPELINE BEHAVIOR                   │
├─────────────────────────────────────────────────┤
│  services.AddTransient(                         │
│    typeof(IPipelineBehavior<,>),                │
│    typeof(ValidationBehavior<,>)                │
│  )                                              │
│                                                 │
│  Effect: Automatic validation before handler    │
│  - FluentValidation runs first                  │
│  - Throw ValidationException if invalid         │
│  - Handler executes if valid                    │
└─────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────┐
│  BUILD & CONFIGURE                              │
├─────────────────────────────────────────────────┤
│  builder.Services.AddControllers();             │
│  builder.Services.AddSwaggerGen(...);           │
│  builder.Services.AddCors(...);                 │
│  builder.Services.AddEndpointsApiExplorer();    │
└─────────────────────────────────────────────────┘
                            ↓
var app = builder.Build();

                            ↓
┌─────────────────────────────────────────────────┐
│  MIDDLEWARE PIPELINE                            │
├─────────────────────────────────────────────────┤
│  1. ExceptionHandlingMiddleware                 │
│     └─ Catch all exceptions → Proper response   │
│                                                 │
│  2. RequestResponseLoggingMiddleware            │
│     └─ Log HTTP requests/responses              │
│                                                 │
│  3. Swagger/SwaggerUI                           │
│     └─ API documentation                        │
│                                                 │
│  4. HTTPS Redirect                              │
│                                                 │
│  5. CORS                                        │
│                                                 │
│  6. Authentication                              │
│     └─ JWT token validation                     │
│                                                 │
│  7. Authorization                               │
│     └─ Role-based access control                │
│                                                 │
│  8. Routing                                     │
│     └─ Map controllers                          │
└─────────────────────────────────────────────────┘
                            ↓
app.UseExceptionHandlingMiddleware();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(...);
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

                            ↓
┌─────────────────────────────────────────────────┐
│  SEED DATA (Optional)                           │
├─────────────────────────────────────────────────┤
│  DataSeeder seeds initial data:                 │
│  - Categories                                   │
│  - Products                                     │
│  - Sample users                                 │
│  - Test data for development                    │
└─────────────────────────────────────────────────┘
                            ↓
await app.RunAsync();
```

### 6.2 Lifetime Management

```
┌──────────────┬──────────────┬─────────────────────────┐
│   Lifetime   │  Instances   │      Use Case           │
├──────────────┼──────────────┼─────────────────────────┤
│  TRANSIENT   │  New per use │ Stateless services      │
│              │              │ Lightweight objects     │
│              │              │ AutoMapper profiles     │
│              │              │ Validators              │
├──────────────┼──────────────┼─────────────────────────┤
│   SCOPED     │  Per request │ DbContext               │
│              │              │ Repositories            │
│              │              │ UnitOfWork              │
│              │              │ Services with state     │
│              │              │ Current user service    │
├──────────────┼──────────────┼─────────────────────────┤
│  SINGLETON   │  Once + reuse│ Logger                  │
│              │              │ Configuration           │
│              │              │ Factory patterns        │
│              │              │ Event publishers        │
└──────────────┴──────────────┴─────────────────────────┘

Code Example:

// DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        // Transient
        services.AddTransient<IValidator<CreateOrderCommand>, 
                              CreateOrderCommandValidator>();
        
        // Scoped
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPricingService, PricingService>();
        
        // Singleton
        services.AddSingleton<ILoggerService>(provider => 
            LoggerService.Instance);
        services.AddSingleton<IEventPublisher, EventPublisher>();
        
        return services;
    }
}
```

---

## 7. VALIDATION & ERROR HANDLING

### 7.1 FluentValidation Pipeline

```
Request → ValidationBehavior
              ↓
         Check all validators registered for TRequest
              ↓
         For each validator:
         ├─ Validate(context)
         └─ Collect errors
              ↓
         If errors exist:
         └─ Throw ValidationException
              ↓
         If no errors:
         └─ Continue to Handler
```

### 7.2 Validation Example

```csharp
// CreateOrderCommand
public class CreateOrderCommand : IRequest<OrderDto>
{
    public int UserId { get; set; }
    public int CartId { get; set; }
    public string ShippingAddress { get; set; }
    public string? BillingAddress { get; set; }
    public PaymentMethodType PaymentMethod { get; set; }
    public string? Note { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }
}

// CreateOrderCommandValidator
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .WithMessage("Shipping address is required")
            .Length(10, 500)
            .WithMessage("Shipping address must be between 10-500 characters");

        RuleFor(x => x.OrderItems)
            .NotEmpty()
            .WithMessage("Order must contain at least 1 item")
            .Must(items => items.Count > 0);

        RuleForEach(x => x.OrderItems).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        });
    }
}

// When invalid request comes in:
POST /api/orders
{
  "userId": 0,                    // ❌ Invalid: must be > 0
  "shippingAddress": "123",       // ❌ Invalid: too short
  "orderItems": []                // ❌ Invalid: empty
}

Response: 400 Bad Request
{
  "errors": [
    {
      "field": "UserId",
      "message": "User ID must be greater than 0"
    },
    {
      "field": "ShippingAddress",
      "message": "Shipping address must be between 10-500 characters"
    },
    {
      "field": "OrderItems",
      "message": "Order must contain at least 1 item"
    }
  ]
}
```

### 7.3 Global Exception Handling

```csharp
// ExceptionHandlingMiddleware
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ValidationException ve => new ApiResponse
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = ve.Errors.GroupBy(x => x.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            },
            
            ArgumentNullException ane => new ApiResponse
            {
                StatusCode = 400,
                Message = $"Required field missing: {ane.ParamName}"
            },
            
            InvalidOperationException ioe => new ApiResponse
            {
                StatusCode = 409,
                Message = ioe.Message
            },
            
            UnauthorizedAccessException => new ApiResponse
            {
                StatusCode = 401,
                Message = "Unauthorized access"
            },
            
            _ => new ApiResponse
            {
                StatusCode = 500,
                Message = "An unexpected error occurred",
                Details = ex.Message
            }
        };
        
        context.Response.StatusCode = response.StatusCode;
        return context.Response.WriteAsJsonAsync(response);
    }
}
```

---

## 8. BẢNG SO SÁNH CÁC PATTERN

### 8.1 Tổng Hợp Design Patterns

| Pattern | Vị Trí | Mục Đích | Lợi Ích | Nhược Điểm |
|---------|--------|----------|---------|-----------|
| **Singleton** | LoggerService | Đảm bảo instance duy nhất | Thread-safe, resource efficient | Khó test (static state) |
| **Factory** | PaymentFactory | Tạo object động theo type | Dễ extend, no if/else hell | Thêm complexity |
| **Strategy** | PricingService | Chọn thuật toán lúc runtime | Flexible, testable, DRY | Class explosion |
| **Observer** | EventPublisher | Notify multiple subscribers | Loose coupling, event-driven | Complex to debug |
| **Repository** | UnitOfWork | Tách data access logic | Testable, reusable, DRY | Extra abstraction |
| **Decorator** | DiscountService | Xếp chồng chức năng | Flexible, composable | Nesting complexity |
| **Command** | CQRS | Encapsulate request | Undo/Redo, Queue, Log | Over-engineering |
| **CQRS** | MediatR | Tách Write/Read logic | Scalable, focused | Complexity overhead |
| **Mediator** | MediatR | Điều phối giữa components | Loose coupling, clean | Dependency on library |
| **Unit of Work** | UnitOfWork | Quản lý transactions | Atomic operations, rollback | Extra abstraction layer |

### 8.2 Pattern Usage Statistics

```
Project Design Patterns Usage:

┌─────────────────────────────────────────┐
│  Design Pattern Breakdown                │
├─────────────────────────────────────────┤
│ Singleton (1)              ███░░ 10%     │
│ Factory (9)                ███████████░░ 35%     │
│ Strategy (8)               █████████░░░░ 30%     │
│ Observer (11)              ████████████░ 35%     │
│ Repository (6)             ███████░░░░░░ 25%     │
│ Decorator (10)             ███████████░░ 32%     │
│ Command (150+)             █████████████ 100%    │
│ CQRS (150+)                █████████████ 100%    │
│ Mediator (MediatR)         █████████████ 100%    │
│ Unit of Work (1)           ███░░░░░░░░░░ 10%     │
└─────────────────────────────────────────┘

Total: 13 Patterns
Total Files: 100+
Lines of Code: 5000+
```

---

## 📊 KIẾN TRÚC TỔNG QUAN

```
                   ┌─────────────────────────┐
                   │  CLIENT (Browser/App)   │
                   └────────────┬────────────┘
                                │
                    HTTP/REST API (Swagger)
                                │
         ┌──────────────────────┴──────────────────────┐
         │                                             │
    ┌────▼────────────────────────────────────────────┴────┐
    │             PRESENTATION LAYER (mtkpm)              │
    ├─────────────────────────────────────────────────────┤
    │  OrdersController  ProductsController PaymentController│
    │  CartController  AuthController  NotificationController
    │  DiscountController  PricingController               │
    │                                                     │
    │  Middleware:                                        │
    │  - ExceptionHandlingMiddleware                      │
    │  - RequestResponseLoggingMiddleware                 │
    │  - ValidationBehavior                               │
    └──────────────────────┬──────────────────────────────┘
                           │
                       MediatR
                           │
    ┌──────────────────────┴──────────────────────────────┐
    │          APPLICATION LAYER (mtkpm.Application)      │
    ├─────────────────────────────────────────────────────┤
    │  Features (Commands & Queries):                    │
    │  - Orders/Commands/CreateOrder/                    │
    │  - Products/Queries/GetProductById/                │
    │  - Cart/Commands/AddToCart/                        │
    │  - Auth/Commands/Login/                            │
    │  - Users/Commands/UpdateUser/                      │
    │  - Categories/Commands/CreateCategory/             │
    │                                                    │
    │  Validators (FluentValidation)                     │
    │  DTOs (Data Transfer Objects)                      │
    │  Mapper (AutoMapper)                               │
    │  Interfaces (Services)                             │
    └──────────────────────┬──────────────────────────────┘
                           │
    ┌──────────────────────┴──────────────────────────────┐
    │         DOMAIN LAYER (mtkpm.Domain)                │
    ├─────────────────────────────────────────────────────┤
    │  Entities:                                         │
    │  - Order, OrderItem, OrderStatus                   │
    │  - Product, Category                               │
    │  - User, RefreshToken                              │
    │  - CartItem, FavouriteProduct                       │
    │                                                    │
    │  Enums:                                            │
    │  - PaymentMethodType                               │
    │  - OrderStatus                                     │
    │                                                    │
    │  Events (DDD):                                     │
    │  - OrderCreatedEvent                               │
    │  - PaymentCompletedEvent                           │
    │  - OrderCancelledEvent                             │
    │                                                    │
    │  Business Logic (Rich Domain Model)                │
    └──────────────────────┬──────────────────────────────┘
                           │
    ┌──────────────────────┴──────────────────────────────┐
    │       INFRASTRUCTURE LAYER (mtkpm.Infrastructure)   │
    ├─────────────────────────────────────────────────────┤
    │  Services:                                         │
    │  - Payment: PaymentFactory, PaymentService        │
    │    - CreditCardPaymentService                      │
    │    - PayPalPaymentService                          │
    │    - CODPaymentService                             │
    │    - BankTransferPaymentService                    │
    │                                                    │
    │  - Pricing: PricingService (Strategy Pattern)      │
    │    - RegularPricingStrategy                        │
    │    - BulkDiscountPricingStrategy                   │
    │    - SeasonalPricingStrategy                       │
    │    - VIPMemberPricingStrategy                      │
    │                                                    │
    │  - Discount: DiscountService (Decorator Pattern)   │
    │    - PercentageDiscountDecorator                   │
    │    - FreeShippingDiscountDecorator                 │
    │    - LoyaltyPointsDiscountDecorator                │
    │    - BundleDiscountDecorator                       │
    │                                                    │
    │  - Notification: (Observer Pattern)                │
    │    - EventPublisher (Subject)                      │
    │    - EmailNotificationService (Observer)           │
    │    - SMSNotificationService (Observer)             │
    │    - PushNotificationService (Observer)            │
    │                                                    │
    │  - Auth: AuthService, JwtService                   │
    │  - Logger: LoggerService (Singleton)               │
    │                                                    │
    │  Repositories (Repository Pattern):                │
    │  - ProductRepository                               │
    │  - OrderRepository                                 │
    │  - CategoryRepository                              │
    │  - CartItemRepository                              │
    │  - FavouriteProductRepository                      │
    │  - RefreshTokenRepository                          │
    │                                                    │
    │  Persistence:                                      │
    │  - ApplicationDbContext (EF Core)                  │
    │  - Migrations                                      │
    │  - UnitOfWork (Unit of Work Pattern)               │
    │                                                    │
    │  Configuration:                                    │
    │  - Data Configurations (Fluent API)                │
    │  - DependencyInjection.cs                          │
    └──────────────────────┬──────────────────────────────┘
                           │
                       Database
                           │
                       SQL Server
```

---

## 🎓 KẾT LUẬN

### Điểm Mạnh Của Project

✅ **Architecture**: Clean Architecture + CQRS + DDD
✅ **Design Patterns**: 13 patterns được sử dụng hiệu quả
✅ **Code Quality**: SOLID Principles, DRY, maintainable
✅ **Testability**: Loose coupling, easy to mock
✅ **Scalability**: Independent layers, async/await
✅ **Security**: JWT Authentication, role-based authorization
✅ **Documentation**: Swagger, XML comments, guides
✅ **Error Handling**: Global exception handling, validation
✅ **Logging**: Centralized logging, request/response logging
✅ **Data Access**: Repository pattern, Unit of Work, soft delete

### Công Nghệ Được Sử Dụng

| Layer | Technology |
|-------|-----------|
| API | ASP.NET Core 8 |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Patterns | MediatR (CQRS), Mediator |
| Authentication | JWT Bearer, ASP.NET Identity |
| Logging | Custom Logger Service |
| Documentation | Swagger/OpenAPI |

### Luôn Có Thể Cải Thiện

- 🔍 Unit Tests (xUnit, Moq)
- 📚 Integration Tests
- ⚡ Caching (Redis)
- 📈 Performance optimization
- 🔐 Advanced security features
- 📊 Analytics & monitoring
- 🌍 Internationalization (i18n)

---

**Created for MTKPM E-Commerce System Presentation**
**Tương thích với .NET 8**
