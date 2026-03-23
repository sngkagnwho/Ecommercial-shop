# ??? KI?N TRÚC VÀ DESIGN PATTERNS NÂNG CAO - MTKPM

## 8?? CQRS PATTERN - Command Query Responsibility Segregation

### ?? V? Trí
```
mtkpm.Application/Features/
- Orders/Commands/ (Command handlers)
- Orders/Queries/ (Query handlers)
- Products/Commands/ (Command handlers)
- Products/Queries/ (Query handlers)
```

### ?? Gi?i Thích
**CQRS Pattern** **tách bi?t Command (Write)** và **Query (Read)** logic ??:
- T?i ?u hóa performance
- D? scale independent
- Clear separation of concerns

### ?? Cách Ho?t ??ng
```
Client Request
        ?
    ??????????????????
    ?                ?
Command          Query
(Write)          (Read)
    ?                ?
Database         Cache/View
```

### ?? ?ng D?ng Th?c T?
- **Commands**: CreateOrderCommand, CancelOrderCommand
- **Queries**: GetOrderByIdQuery, GetUserOrdersQuery
- Có th? dùng database khác cho Query (read model)

### ?? Flow
```
1. User t?o ??n ? CreateOrderCommand ? Write to Database
2. User xem danh sách ? GetUserOrdersQuery ? Read from Cache/View
3. Tách bi?t giúp optimize t?ng ph?n riêng
```

---

## 9?? MEDIATOR PATTERN (MediatR) - Request/Handler Abstraction

### ?? V? Trí
```
mtkpm/Program.cs (DI Registration)
mtkpm.Application/Features/Orders/Commands/CreateOrder/CreateOrderCommandHandler.cs
mtkpm.Application/Features/Orders/Queries/GetOrderById/GetOrderByIdQueryHandler.cs
```

### ?? Gi?i Thích
**MediatR** là library implementation c?a **Mediator Pattern** ?? ?i?u ph?i Request/Handler mà không c?n coupling.

### ?? Cách Ho?t ??ng
```
Controller
    ?
IMediator.Send(Request)
    ?
MediatR tìm Handler phù h?p
    ?
Handler x? lý
    ?
Tr? v? Response
```

### ?? ?ng D?ng Th?c T?
```csharp
// Controller
var result = await _mediator.Send(new CreateOrderCommand { ... });

// MediatR t? ??ng tìm CreateOrderCommandHandler
// Không c?n bi?t handler c? th?
```

### ?? Flow
```
1. POST /api/orders
2. Controller: _mediator.Send(CreateOrderCommand)
3. MediatR: Tìm IRequestHandler<CreateOrderCommand, OrderDto>
4. G?i CreateOrderCommandHandler.Handle()
5. Tr? v? OrderDto
```

### ?? L?i Ích
- ? Loose coupling
- ? Easy to test (mock mediator)
- ? Clear command/query separation
- ? Pipelines (validation, logging, etc.)

---

## ?? UNIT OF WORK PATTERN - Transaction Management

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/Repositories/IUnitOfWork.cs
mtkpm.Infrastructure/Persistence/UnitOfWork.cs
```

### ?? Gi?i Thích
**Unit of Work Pattern** qu?n lý **t?t c? database operations** nh? m?t transaction duy nh?t.

### ?? Cách Ho?t ??ng
```
UnitOfWork
    ??? Products Repository
    ??? Orders Repository
    ??? Users Repository
    ??? SaveChangesAsync() ? Commit All
```

### ?? ?ng D?ng Th?c T?
```csharp
// T?o ??n hàng + c?p nh?t t?n kho + ghi log
public async Task<bool> Handle(CreateOrderCommand request)
{
    var order = new Order(...);
    _unitOfWork.Orders.Add(order);
    
    foreach (var item in order.Items)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
        product.DecreaseStock(item.Quantity);
        _unitOfWork.Products.Update(product);
    }
    
    await _unitOfWork.SaveChangesAsync();
    // N?u l?i ? Rollback t?t c?
}
```

### ?? Flow
```
1. B?t ??u transaction
2. Thêm order vào database
3. C?p nh?t stock s?n ph?m
4. Ghi log
5. SaveChangesAsync() ? Commit ho?c Rollback
```

---

## ??? CLEAN ARCHITECTURE - Ki?n Trúc S?ch

### ?? C?u Trúc Project

```
mtkpm (Presentation Layer)
??? Controllers/
?   ??? ProductsController.cs
?   ??? OrdersController.cs
?   ??? PaymentController.cs
?   ??? ...
??? Middleware/
??? Program.cs (DI Setup)
??? appsettings.json

mtkpm.Application (Application Layer)
??? Common/
?   ??? Interfaces/
?   ??? DTOs/
?   ??? Mappings/
??? Features/
?   ??? Orders/
?   ?   ??? Commands/
?   ?   ??? Queries/
?   ?   ??? Handlers/
?   ??? Products/
?   ??? ...
??? Validators/

mtkpm.Infrastructure (Infrastructure Layer)
??? Services/
?   ??? Payments/
?   ??? Notifications/
?   ??? Pricing/
?   ??? Discounts/
?   ??? ...
??? Persistence/ (Database)
??? DependencyInjection.cs
??? Middleware/

mtkpm.Domain (Domain Layer)
??? Entities/
?   ??? Business/
?   ??? Identity_Auth/
??? Events/
??? Enums/
??? ValueObjects/
```

### ?? L?p Ki?n Trúc

| L?p | Trách Nhi?m | Ví D? |
|-----|-----------|--------|
| **Domain** | Business logic, Entities | Order, Product, Payment |
| **Application** | Use cases, Commands, Queries | CreateOrderCommand |
| **Infrastructure** | External services, Database | PaymentService, Logger |
| **Presentation** | API endpoints, User interaction | Controllers |

### ?? Flow Qua Các L?p

```
Request (HTTP)
    ? (Presentation)
Controller
    ? (DI + MediatR)
Handler (Application)
    ? (Domain Logic)
Service (Infrastructure)
    ? (Database)
Repository (Infrastructure)
    ? (Database Access)
Entity (Domain)
    ?
Response (JSON)
```

### ?? L?i Ích Clean Architecture

- ? **Independence of Frameworks**: Có th? thay ??i framework mà không ?nh h??ng logic
- ? **Testability**: M?i l?p có th? test ??c l?p
- ? **Independence of UI**: Logic không ph? thu?c UI
- ? **Independence of Database**: Logic không ph? thu?c database
- ? **Independence of Any External Agency**: Logic không ph? thu?c external service

---

## ?? DEPENDENCY INJECTION (DI) - IoC Container

### ?? V? Trí
```
mtkpm/Program.cs (Main DI Setup)
mtkpm.Infrastructure/DependencyInjection.cs (Infrastructure Services)
mtkpm.Application/DependencyInjection.cs (Application Services)
```

### ?? Gi?i Thích
**Dependency Injection** giúp **qu?n lý dependencies** t? ??ng mà không c?n t?o manual.

### ?? Cách Ho?t ??ng
```
Program.cs
??? AddApplicationServices()
?   ??? MediatR
?   ??? Validators
?   ??? AutoMapper
??? AddInfrastructureServices()
?   ??? Database Context
?   ??? UnitOfWork
?   ??? Repositories
?   ??? Payment Services
?   ??? Notification Services
?   ??? Pricing Services
??? AddPersistence()
    ??? DbContext
```

### ?? ?ng D?ng Th?c T?

```csharp
// Program.cs
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(configuration);

// DependencyInjection.cs (Infrastructure)
public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    // Database
    services.AddDbContext<ApplicationDbContext>();
    
    // Unit of Work
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    // Repositories
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    
    // Services
    services.AddScoped<IPaymentFactory, PaymentFactory>();
    services.AddScoped<IPaymentService, PaymentService>();
    services.AddScoped<IPricingService, PricingService>();
    services.AddScoped<IDiscountService, DiscountService>();
    services.AddScoped<IEventPublisher, EventPublisher>();
    services.AddSingleton<ILoggerService, LoggerService>();
    
    return services;
}

// Controller (Auto-inject dependencies)
public class OrdersController : ControllerBase
{
    public OrdersController(IMediator mediator) // DI t? ??ng
    {
        _mediator = mediator;
    }
}
```

### ?? Lifetime Management

| Lifetime | Mô T? | S? D?ng |
|----------|------|--------|
| **Transient** | T?o m?i m?i l?n request | Stateless services |
| **Scoped** | T?o m?i m?i HTTP request | Repository, DbContext |
| **Singleton** | T?o m?t l?n, dùng mãi | Logger, Configuration |

```csharp
services.AddTransient<ITransientService, TransientService>();
services.AddScoped<IScopedService, ScopedService>();
services.AddSingleton<ISingletonService, SingletonService>();
```

---

## 1??1?? DOMAIN-DRIVEN DESIGN (DDD) - Domain-Centric Architecture

### ?? V? Tr?
```
mtkpm.Domain/
??? Entities/ (Order, Product, User)
??? ValueObjects/ (Money, Quantity)
??? Events/ (OrderCreatedEvent, PaymentCompletedEvent)
??? Aggregates/ (Order is root aggregate)
```

### ?? Gi?i Thích
**DDD** t?p trung vào **Domain Logic** thay vì Technical Details.

### ?? ?ng D?ng Th?c T?

```csharp
// Domain/Entities/Order.cs (Rich Domain Model)
public class Order : BaseEntity
{
    public string OrderNumber { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    
    // Business Logic (không ph?i dùng service)
    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot update completed order");
        
        Status = newStatus;
        // Phát event (Observer Pattern)
        RaiseDomainEvent(new OrderStatusChangedEvent(Id, newStatus));
    }
    
    public decimal GetTotal()
    {
        return _orderItems.Sum(x => x.TotalPrice);
    }
    
    public void AddOrderItem(OrderItem item)
    {
        _orderItems.Add(item);
    }
}
```

### ?? Ubiquitous Language
- **Order** (??n hàng): Aggregate Root
- **OrderItem** (Chi ti?t ??n): Entity
- **OrderStatus** (Tr?ng thái): Value Object
- **OrderCreatedEvent** (S? ki?n): Domain Event

---

## 1??2?? VALIDATION PATTERN - FluentValidation

### ?? V? Trí
```
mtkpm.Application/Features/*/Commands/*Validator.cs
mtkpm.Application/Common/DTOs/*/Validators/
```

### ?? Gi?i Thích
**Fluent Validation** cung c?p **declarative validation rules** cho DTOs và Commands.

### ?? ?ng D?ng Th?c T?

```csharp
// CreateOrderCommandValidator.cs
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID không h?p l?");

        RuleFor(x => x.ShippingAddress)
            .NotEmpty().WithMessage("??a ch? giao hàng là b?t bu?c")
            .Length(10, 500).WithMessage("?? dài ??a ch? 10-500 ký t?");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("??n hàng ph?i có ít nh?t 1 s?n ph?m")
            .Must(items => items.Count > 0);

        RuleForEach(x => x.OrderItems).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).GreaterThan(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}

// T? ??ng validate trong handler
public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
{
    // MediatR pipeline t? ??ng validate tr??c khi g?i handle
    // N?u invalid ? throw ValidationException
    ...
}
```

### ?? Pipeline Validation

```
Request
    ?
Validation Middleware
    ? (N?u invalid) ? Throw ValidationException
    ? (N?u valid) ? Continue
Handler
    ?
Response
```

---

## 1??3?? LOGGING & MONITORING - Centralized Logging

### ?? V? Trí
```
mtkpm.Infrastructure/Services/LoggerService.cs
mtkpm/Middleware/RequestResponseLoggingMiddleware.cs
mtkpm.Application/Common/Interfaces/ILoggerService.cs
```

### ?? Gi?i Thích
**Centralized Logging** ghi t?t c? activity (requests, errors, business events) vào m?t n?i.

### ?? ?ng D?ng Th?c T?

```csharp
// LoggerService.cs (Singleton)
public class LoggerService : ILoggerService
{
    public void LogInfo(string message, string category)
    {
        // Ghi vào file/database/external service
        Console.WriteLine($"[{category}] {message}");
    }
    
    public void LogError(string message, Exception ex)
    {
        Console.WriteLine($"[ERROR] {message}: {ex.Message}");
    }
}

// Usage
_logger.LogInfo("Order created successfully", "OrderService");
_logger.LogError("Payment processing failed", exception);

// RequestResponseLoggingMiddleware
// T? ??ng log t?t c? requests/responses
public async Task InvokeAsync(HttpContext context)
{
    var request = context.Request;
    var body = await request.GetBodyAsStringAsync();
    
    _logger.LogInfo($"REQUEST: {request.Method} {request.Path} - Body: {body}", "HTTP");
    
    await _next(context);
    
    _logger.LogInfo($"RESPONSE: {context.Response.StatusCode}", "HTTP");
}
```

### ?? Logging Levels
- ? **Info**: Business events
- ?? **Warning**: Potential issues
- ? **Error**: Exceptions
- ?? **Debug**: Development info

---

## ?? ARCHITECTURE OVERVIEW DIAGRAM

```
???????????????????????????????????????????????????
?         PRESENTATION LAYER (Controllers)        ?
???????????????????????????????????????????????????
?     POST /api/orders ? OrdersController         ?
?            ? (DI: IMediator)                    ?
???????????????????????????????????????????????????
?      APPLICATION LAYER (CQRS + MediatR)        ?
???????????????????????????????????????????????????
?   CreateOrderCommand ? CreateOrderCommandHandler ?
?            ? (DI: IUnitOfWork, Services)       ?
???????????????????????????????????????????????????
?      DOMAIN LAYER (Business Logic)              ?
???????????????????????????????????????????????????
?   Order Aggregate ? Validate ? RaiseDomainEvent?
?            ? (Repository Pattern)               ?
???????????????????????????????????????????????????
?    INFRASTRUCTURE LAYER (Services)              ?
???????????????????????????????????????????????????
?  Database ? UnitOfWork ? Repositories           ?
?  PaymentService ? PaymentFactory                ?
?  PricingService ? Strategies                    ?
?  DiscountService ? Decorators                   ?
?  EventPublisher ? Observers (Notifications)    ?
?  LoggerService (Singleton)                      ?
???????????????????????????????????????????????????
?         DATABASE (Entity Framework)             ?
???????????????????????????????????????????????????
```

---

## ? ?I?M M?NH C?A PROJECT

### ?? Ki?n Trúc
- ? **Clean Architecture**: Tách bi?t rõ ràng gi?a các l?p
- ? **DDD**: Domain-centric design
- ? **CQRS**: Tách Query và Command
- ? **Dependency Injection**: Loose coupling
- ? **Unit of Work**: Transaction management

### ?? Design Patterns
- ? **7 Gang of Four Patterns**: Singleton, Factory, Strategy, Observer, Repository, Decorator, Command
- ? **MediatR**: Mediator pattern implementation
- ? **Fluent Validation**: Declarative validation

### ?? Security & Reliability
- ? **Authorization**: Role-based access control
- ? **Validation**: Input validation at multiple layers
- ? **Error Handling**: Global exception handling
- ? **Logging**: Centralized logging

### ?? Scalability
- ? **Async/Await**: Non-blocking operations
- ? **Caching**: Performance optimization
- ? **Event-Driven**: Loose coupling with Observer pattern
- ? **Strategy Pattern**: Easy algorithm switching

### ?? Testability
- ? **Dependency Injection**: Easy to mock
- ? **Repository Pattern**: Easy to test data access
- ? **Clear Separation**: Each layer testable independently
- ? **Validators**: Business rules testable

### ?? Maintainability
- ? **Clear Folder Structure**: Easy to navigate
- ? **Consistent Naming**: Self-documenting code
- ? **SOLID Principles**: S, O, L, I, D all followed
- ? **DRY**: No code duplication

### ?? Documentation
- ? **XML Comments**: API documentation
- ? **Swagger**: Auto-generated API docs
- ? **Design Patterns Guide**: This file

---

## ?? SOLID PRINCIPLES IMPLEMENTATION

| Principle | Implementation | Ví D? |
|-----------|----------------|-------|
| **S**ingle Responsibility | M?i class m?t trách nhi?m | LoggerService ch? log |
| **O**pen/Closed | Open for extension, closed for modification | Decorator pattern cho discounts |
| **L**iskov Substitution | Subclass có th? thay parent | IPricingStrategy implementations |
| **I**nterface Segregation | Specific interfaces | IPaymentMethod instead of generic |
| **D**ependency Inversion | Depend on abstractions | IRepository instead of DbContext |

---

## ?? K?T LU?N

Project MTKPM s? d?ng:

### **Design Patterns**: 
- 7 Gang of Four patterns
- MediatR (Mediator)
- CQRS
- DDD

### **Architecture**:
- Clean Architecture
- Dependency Injection
- Layered Architecture

### **Best Practices**:
- SOLID Principles
- Fluent Validation
- Centralized Logging
- Error Handling
- Authorization

### **K?t Qu?**:
- ? Highly maintainable
- ? Highly testable
- ? Highly scalable
- ? Production-ready
- ? Enterprise-grade architecture
