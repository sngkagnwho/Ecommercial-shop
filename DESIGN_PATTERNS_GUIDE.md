# ?? DESIGN PATTERNS TRONG PROJECT MTKPM

## ?? T?ng Quan Các Pattern S? D?ng

| Pattern | ?ng D?ng | Ch?c N?ng |
|---------|----------|----------|
| **Singleton** | Logger | Qu?n lý logging duy nh?t |
| **Factory** | Payment | T?o các ph??ng th?c thanh toán |
| **Strategy** | Pricing | Tính giá theo nhi?u chi?n l??c |
| **Observer** | Notification | Thông báo s? ki?n ??n hàng |
| **Repository** | Data Access | Truy c?p d? li?u |
| **Decorator** | Discount | X?p ch?ng chi?t kh?u |
| **Command** | Order | X? lý các l?nh t?o/h?y ??n hàng |
| **CQRS** | Separation | Tách Query và Command |
| **Mediator** | MediatR | ?i?u ph?i gi?a Request/Handler |
| **Unit of Work** | Transaction | Qu?n lý transaction |

---

## 1?? SINGLETON PATTERN - Logger

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/ILoggerService.cs
mtkpm.Infrastructure/Services/LoggerService.cs
```

### ?? Gi?i Thích
**Singleton Pattern** ??m b?o ch? có **m?t instance duy nh?t** c?a class Logger trong toàn b? ?ng d?ng.

### ?? Cách Ho?t ??ng
```
Client 1 ? 
Client 2 ? LoggerService (Instance Duy Nh?t)
Client 3 ?
```

### ?? ?ng D?ng Th?c T?
- Ghi log các ho?t ??ng: Login, Payment, Order
- ??m b?o t?t c? log ???c ghi vào **m?t n?i duy nh?t**
- C?u hình log centralized

### ?? Flow
```
1. User ??ng nh?p ? Logger.LogInfo("User logged in")
2. User thanh toán ? Logger.LogInfo("Payment processed")
3. User t?o ??n ? Logger.LogInfo("Order created")
4. ? T?t c? log ???c ghi vào file/database duy nh?t
```

---

## 2?? FACTORY PATTERN - Payment Methods

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/Services/IPaymentFactory.cs
mtkpm.Infrastructure/Services/Payments/PaymentFactory.cs
- CreditCardPaymentService.cs
- PayPalPaymentService.cs
- CODPaymentService.cs
- BankTransferPaymentService.cs
```

### ?? Gi?i Thích
**Factory Pattern** giúp **t?o các object phù h?p** mà không c?n bi?t class c? th?.

### ?? Cách Ho?t ??ng
```
User ch?n ph??ng th?c thanh toán
        ?
PaymentFactory.CreatePaymentMethod(type)
        ?
    ???????????????????????????????
    ?        ?         ?          ?
Credit    PayPal     COD       Bank
Card      Payment    Payment   Transfer
```

### ?? ?ng D?ng Th?c T?
- User ch?n "Credit Card" ? T?o CreditCardPaymentService
- User ch?n "PayPal" ? T?o PayPalPaymentService
- User ch?n "COD" ? T?o CODPaymentService

### ?? Flow
```
1. User ch?n ph??ng th?c thanh toán (Dropdown)
2. System g?i: PaymentFactory.CreatePaymentMethod("CREDIT_CARD")
3. Factory tr? v?: CreditCardPaymentService instance
4. G?i PaymentService.ProcessPaymentAsync()
5. Th?c hi?n xác th?c + x? lý thanh toán
6. Tr? v? k?t qu? (Success/Failed)
```

### ?? Code Flow
```csharp
// PaymentFactory.cs
public IPaymentMethod CreatePaymentMethod(PaymentMethodType type)
{
    return type switch
    {
        PaymentMethodType.CreditCard => new CreditCardPaymentService(_logger),
        PaymentMethodType.PayPal => new PayPalPaymentService(_logger),
        PaymentMethodType.COD => new CODPaymentService(_logger),
        PaymentMethodType.BankTransfer => new BankTransferPaymentService(_logger),
        _ => throw new InvalidOperationException("Unknown payment method")
    };
}

// Controller
var paymentMethod = _paymentFactory.CreatePaymentMethod(request.PaymentMethod);
var result = await paymentMethod.ProcessPaymentAsync(amount);
```

---

## 3?? STRATEGY PATTERN - Pricing

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/Services/IPricingStrategy.cs
mtkpm.Infrastructure/Services/Pricing/
- RegularPricingStrategy.cs (Giá th??ng)
- BulkDiscountPricingStrategy.cs (Gi?m s? l??ng)
- SeasonalPricingStrategy.cs (Giá mùa v?)
- VIPMemberPricingStrategy.cs (Giá VIP)
```

### ?? Gi?i Thích
**Strategy Pattern** cho phép **ch?n thu?t toán lúc runtime** mà không thay ??i client code.

### ?? Cách Ho?t ??ng
```
Product (Price = 100.000 ?)
        ?
    Ch?n Strategy
        ?
    ???????????????????????????????
    ?        ?          ?         ?
Regular   Bulk      Seasonal    VIP
(100k)    (90k)     (85k)      (75k)
```

### ?? ?ng D?ng Th?c T?
- **Regular**: Giá bán th??ng (100%)
- **Bulk**: Mua 10+ s?n ph?m ???c gi?m 10%
- **Seasonal**: Black Friday/T?t gi?m 15-25%
- **VIP**: Bronze 5%, Silver 10%, Gold 15%, Platinum 25%

### ?? Flow
```
1. User xem s?n ph?m ? Price = 100.000 ?
2. System ki?m tra user tier + ngày tháng + s? l??ng
3. Ch?n strategy phù h?p:
   - N?u là VIP Gold ? VIPMemberPricingStrategy
   - N?u là Black Friday ? SeasonalPricingStrategy
   - N?u mua 10+ ? BulkDiscountPricingStrategy
   - M?c ??nh ? RegularPricingStrategy
4. G?i strategy.CalculatePrice(product, quantity)
5. Tr? v? giá cu?i cùng
```

### ?? Code Flow
```csharp
// PricingService.cs
public decimal CalculatePrice(Product product, int quantity, IPricingStrategy strategy)
{
    var context = new PricingContext
    {
        UserTier = "Gold",
        CurrentDate = DateTime.Now,
        IsVipMember = true
    };
    return strategy.CalculatePrice(product, quantity, context);
}

// VIPMemberPricingStrategy.cs
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
```

---

## 4?? OBSERVER PATTERN - Notification

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/Services/IEventPublisher.cs
mtkpm.Application/Common/Interfaces/Services/INotificationObserver.cs
mtkpm.Infrastructure/Services/Notifications/
- EventPublisher.cs (Subject)
- EmailNotificationService.cs (Observer)
- SMSNotificationService.cs (Observer)
- PushNotificationService.cs (Observer)
```

### ?? Gi?i Thích
**Observer Pattern** cho phép **nhi?u observers l?ng nghe s? ki?n** t? m?t Subject mà không c?n coupling.

### ?? Cách Ho?t ??ng
```
                EventPublisher (Subject)
                      ?
        (Khi Order ???c t?o)
                      ?
        ?????????????????????????????
        ?             ?             ?
      Email         SMS           Push
    Observer      Observer      Observer
     (G?i email) (G?i SMS)    (G?i thông báo)
```

### ?? ?ng D?ng Th?c T?
- **Event**: ??n hàng ???c t?o ? T?t c? observer nh?n ???c
- G?i email xác nh?n cho khách hàng
- G?i SMS thông báo
- G?i push notification (n?u có app)
- **M? r?ng d?**: Thêm Slack, Discord, v.v. mà không s?a code c?

### ?? Flow
```
1. User t?o ??n hàng
2. System phát s? ki?n: OrderCreatedEvent
3. EventPublisher nh?n event
4. G?i t?t c? observers:
   - EmailNotificationService.OnOrderCreated() ? G?i email
   - SMSNotificationService.OnOrderCreated() ? G?i SMS
   - PushNotificationService.OnOrderCreated() ? G?i push
5. T?t c? ??u ch?y ??ng th?i/tu?n t? tùy c?u hình
6. User nh?n 3 thông báo cùng lúc
```

### ?? Code Flow
```csharp
// EventPublisher.cs (Subject)
public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : DomainEvent
{
    var tasks = _observers.Select(x => x.OnEventPublished(@event));
    await Task.WhenAll(tasks);
}

// EmailNotificationService.cs (Observer)
public async Task OnEventPublished(DomainEvent @event)
{
    if (@event is OrderCreatedEvent orderEvent)
    {
        await SendEmailAsync($"Order {orderEvent.OrderNumber} created successfully");
    }
}

// SMSNotificationService.cs (Observer)
public async Task OnEventPublished(DomainEvent @event)
{
    if (@event is OrderCreatedEvent orderEvent)
    {
        await SendSMSAsync($"??n hàng {orderEvent.OrderNumber} ???c t?o thành công");
    }
}
```

---

## 5?? DECORATOR PATTERN - Discount

### ?? V? Trí
```
mtkpm.Infrastructure/Services/Discounts/
- BaseDiscount.cs (Component)
- DiscountDecorator.cs (Decorator base)
- PercentageDiscountDecorator.cs (Decorator)
- FixedAmountDiscountDecorator.cs (Decorator)
- FreeShippingDiscountDecorator.cs (Decorator)
- LoyaltyPointsDiscountDecorator.cs (Decorator)
- BundleDiscountDecorator.cs (Decorator)
```

### ?? Gi?i Thích
**Decorator Pattern** cho phép **x?p ch?ng ch?c n?ng** mà không t?o ra quá nhi?u class.

### ?? Cách Ho?t ??ng
```
BaseDiscount (Giá g?c: 100.000)
        ? (Wrap)
PercentageDecorator (Gi?m 10%) ? 90.000
        ? (Wrap)
FreeShippingDecorator (Mi?n ship 50.000) ? 40.000
        ? (Wrap)
LoyaltyPointsDecorator (Dùng 50.000 ?i?m) ? -10.000 (Ng??i dùng tr? ti?n ho?c ???c thêm ti?n)
```

### ?? ?ng D?ng Th?c T?
- User nh?p mã: "PERCENT_10" ? Gi?m 10%
- User nh?p mã: "FREE_SHIP" ? Mi?n phí v?n chuy?n
- User s? d?ng: "LOYALTY_50" ? S? d?ng 50 ?i?m
- **K?t h?p**: Cùng lúc 3 mã trên ? Gi?m giá l?n l??t

### ?? Flow
```
1. User nh?p mã chi?t kh?u: ["percentage_10", "free_shipping", "loyalty_50"]
2. DiscountService xây d?ng chain:
   - Base: 100.000 ?
   - + Percentage 10%: 90.000 ?
   - + Free Shipping 50k: 40.000 ?
   - + Loyalty 50 points: -10.000 (tùy logic)
3. Tr? v? giá cu?i cùng + breakdown chi?t kh?u
4. User th?y: "Giá g?c 100k - 60k chi?t kh?u = 40k"
```

### ?? Code Flow
```csharp
// DiscountService.cs
public IDiscount BuildDiscount(params IDiscount[] discounts)
{
    IDiscount result = new BaseDiscount();
    foreach (var discount in discounts)
    {
        result = new PercentageDiscountDecorator(result, 10m);
        result = new FreeShippingDiscountDecorator(result, 50000m);
        result = new LoyaltyPointsDiscountDecorator(result, 50);
    }
    return result;
}

// Cách s? d?ng
decimal finalPrice = discount.ApplyDiscount(cart);
// 100.000 ? 90.000 ? 40.000 ? ?
```

---

## 6?? REPOSITORY PATTERN - Data Access

### ?? V? Trí
```
mtkpm.Application/Common/Interfaces/Repositories/
- IUnitOfWork.cs
- IProductRepository.cs
- IOrderRepository.cs
- IUserRepository.cs
```

### ?? Gi?i Thích
**Repository Pattern** **tách logic truy c?p d? li?u** kh?i business logic.

### ?? Cách Ho?t ??ng
```
Business Logic (Handler)
        ?
Repository Interface
        ?
    ??????????????
    ?            ?
  Database    Cache
```

### ?? ?ng D?ng Th?c T?
- `_unitOfWork.Products.GetByIdAsync(id)` ? L?y s?n ph?m
- `_unitOfWork.Orders.AddAsync(order)` ? T?o ??n hàng
- `_unitOfWork.SaveChangesAsync()` ? L?u thay ??i

---

## 7?? COMMAND PATTERN - Order Operations

### ?? V? Trí
```
mtkpm.Application/Features/Orders/Commands/
- CreateOrderCommand.cs
- CancelOrderCommand.cs
- UpdateOrderStatusCommand.cs
```

### ?? Gi?i Thích
**Command Pattern** **encapsulate request thành object** ?? có th?:
- Queue requests
- Log requests
- Undo/Redo requests

### ?? Cách Ho?t ??ng
```
User Action (T?o ??n)
        ?
CreateOrderCommand Object
        ?
CQRS Handler
        ?
Process ? Database ? Response
```

### ?? ?ng D?ng Th?c T?
- T?o ??n hàng: CreateOrderCommand
- H?y ??n hàng: CancelOrderCommand
- C?p nh?t tr?ng thái: UpdateOrderStatusCommand

---

## ?? TÓM T?T FLOW HOÀN CH?NH

### ?? Khi User T?o ??n Hàng:
```
1. POST /api/orders
   ?
2. CreateOrderCommand (Command Pattern)
   ?
3. CreateOrderCommandHandler (CQRS)
   ?
4. Repository.AddAsync() (Repository Pattern)
   ?
5. SaveChangesAsync() ? Database
   ?
6. EventPublisher.PublishAsync(OrderCreatedEvent) (Observer Pattern)
   ?
7. Email + SMS + Push notifications (Observer)
   ?
8. Logger.LogInfo() (Singleton)
   ?
9. Response 200 OK
```

### ?? Khi User Thanh Toán:
```
1. POST /api/payment/process
   ?
2. PaymentFactory.CreatePaymentMethod(type) (Factory Pattern)
   ?
3. Receive CreditCardPaymentService instance
   ?
4. ProcessPaymentAsync()
   ?
5. Logger.LogInfo() (Singleton)
   ?
6. Response with TransactionId
```

### ??? Khi User Tính Giá S?n Ph?m:
```
1. POST /api/pricing/calculate
   ?
2. PricingService.CalculateBestPrice() (Strategy Pattern)
   ?
3. L?p qua t?t c? strategies:
   - RegularPricingStrategy
   - BulkDiscountPricingStrategy
   - SeasonalPricingStrategy
   - VIPMemberPricingStrategy
   ?
4. Ch?n giá t?t nh?t
   ?
5. Response with FinalPrice
```

### ?? Khi User Tính Chi?t Kh?u:
```
1. POST /api/discount/calculate
   ?
2. DiscountService.BuildDiscount(codes) (Decorator Pattern)
   ?
3. Chain decorators:
   PercentageDecorator
   ? FreeShippingDecorator
   ? LoyaltyPointsDecorator
   ?
4. ApplyDiscount() l?n l??t
   ?
5. Response with FinalPrice + Breakdown
```

---

## ?? L?i Ích C?a Các Pattern

| Pattern | L?i Ích |
|---------|---------|
| Singleton | Qu?n lý resource duy nh?t, thread-safe |
| Factory | T?o object linh ho?t, d? m? r?ng |
| Strategy | ??i thu?t toán d? dàng, không s?a code c? |
| Observer | Loose coupling, d? thêm observers m?i |
| Repository | Tách data access logic, d? unit test |
| Decorator | X?p ch?ng ch?c n?ng, tránh class explosion |
| Command | Encapsulate actions, d? undo/redo |

---

## ?? K?t Lu?n

Project s? d?ng **7 design patterns** ph? bi?n ??:
- ? Gi?m coupling
- ? T?ng scalability
- ? D? maintenance
- ? D? unit test
- ? D? m? r?ng tính n?ng m?i
