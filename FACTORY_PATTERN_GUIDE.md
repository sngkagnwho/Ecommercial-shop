# Factory Design Pattern - Payment System

## ?? T?ng Quan

H? th?ng thanh toán s? d?ng **Factory Design Pattern** ?? t?o các payment methods khác nhau m?t cách ??ng.

## ??? Ki?n Trúc Clean Architecture

```
???????????????????????????????????????????????????
?  Presentation Layer (API)                       ?
?  - PaymentController                            ?
???????????????????????????????????????????????????
                   ?
???????????????????????????????????????????????????
?  Application Layer (Business Logic)             ?
?  - IPaymentService (Interface)                  ?
?  - IPaymentFactory (Interface)                  ?
?  - ProcessPaymentCommand                        ?
?  - ProcessPaymentCommandHandler                 ?
???????????????????????????????????????????????????
                   ?
???????????????????????????????????????????????????
?  Infrastructure Layer (Implementation)          ?
?  - PaymentService (Implementation)              ?
?  - PaymentFactory (Factory)                     ?
?  - CreditCardPaymentService                     ?
?  - BankTransferPaymentService                   ?
?  - PayPalPaymentService                         ?
?  - CODPaymentService                            ?
???????????????????????????????????????????????????
```

## ?? Các Thành Ph?n Chính

### 1. **Domain Layer** - PaymentMethodType Enum
```csharp
public enum PaymentMethodType
{
    CreditCard = 1,
    DebitCard = 2,
    BankTransfer = 3,
    PayPal = 4,
    COD = 5,
    MobileWallet = 6
}
```

### 2. **Application Layer** - Interfaces

#### IPaymentMethod
??nh ngh?a các ph??ng th?c mà m?i payment method ph?i có:
- `ValidateAsync()` - Xác th?c thông tin thanh toán
- `ProcessPaymentAsync()` - X? lý thanh toán
- `RefundAsync()` - Hoàn ti?n
- `CheckPaymentStatusAsync()` - Ki?m tra tr?ng thái

#### IPaymentFactory
**Factory Interface** - T?o payment methods:
```csharp
public interface IPaymentFactory
{
    IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType);
    bool IsPaymentMethodSupported(PaymentMethodType paymentType);
}
```

#### IPaymentService
Orchestrator Service:
```csharp
public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(...);
    Task<bool> RefundPaymentAsync(...);
    Task<PaymentStatus> GetPaymentStatusAsync(...);
}
```

### 3. **Infrastructure Layer** - Implementations

#### PaymentFactory (Factory Pattern)
```csharp
public class PaymentFactory : IPaymentFactory
{
    public IPaymentMethod CreatePaymentMethod(PaymentMethodType paymentType)
    {
        return paymentType switch
        {
            PaymentMethodType.CreditCard => new CreditCardPaymentService(_logger),
            PaymentMethodType.BankTransfer => new BankTransferPaymentService(_logger),
            PaymentMethodType.PayPal => new PayPalPaymentService(_logger),
            PaymentMethodType.COD => new CODPaymentService(_logger),
            _ => throw new NotSupportedException($"Payment method {paymentType} not supported")
        };
    }
}
```

#### Payment Method Implementations
- **CreditCardPaymentService** - X? lý th? tín d?ng (via Stripe, Square)
- **BankTransferPaymentService** - Chuy?n kho?n ngân hàng
- **PayPalPaymentService** - PayPal integration
- **CODPaymentService** - Thanh toán khi nh?n hàng

### 4. **PaymentService** - Orchestrator
```csharp
public class PaymentService : IPaymentService
{
    // S? d?ng Factory ?? t?o payment method ??ng
    var paymentMethod = _paymentFactory.CreatePaymentMethod(paymentType);
    
    // Validate
    await paymentMethod.ValidateAsync();
    
    // Process
    var result = await paymentMethod.ProcessPaymentAsync(amount);
}
```

## ?? S? D?ng

### 1. Dependency Injection (DependencyInjection.cs)
```csharp
// ??ng ký Factory và Service
services.AddScoped<IPaymentFactory, PaymentFactory>();
services.AddScoped<IPaymentService, PaymentService>();
```

### 2. Trong Controller
```csharp
[HttpPost("process")]
public async Task<IActionResult> ProcessPayment(ProcessPaymentRequest request)
{
    var command = new ProcessPaymentCommand
    {
        OrderId = request.OrderId,
        Amount = request.Amount,
        PaymentMethod = request.PaymentMethod // Enum value
    };
    
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

### 3. Command Handler
```csharp
public async Task<ProcessPaymentResponse> Handle(ProcessPaymentCommand request, ...)
{
    // Dùng PaymentService (which uses Factory internally)
    var paymentResult = await _paymentService.ProcessPaymentAsync(
        request.PaymentMethod,  // Factory s? t?o ?úng payment method
        request.Amount
    );
    
    if (paymentResult.Success)
    {
        order.MarkAsPaid();
    }
}
```

## ? L?i Ích c?a Factory Pattern

| L?i Ích | Chi Ti?t |
|---------|----------|
| **Decoupling** | Controller không c?n bi?t chi ti?t t?ng payment method |
| **Extensibility** | Thêm payment method m?i ch? c?n thêm class + factory case |
| **Maintainability** | Logic t?o objects t?p trung ? m?t ch? |
| **Consistency** | Cách t?o objects có quy trình nh?t ??nh |
| **Testing** | D? mock payment methods ?? test |

## ?? Flow ??y ??

```
Request (PaymentMethodType) 
    ?
ProcessPaymentCommand 
    ?
ProcessPaymentCommandHandler 
    ?
IPaymentService.ProcessPaymentAsync()
    ?
PaymentFactory.CreatePaymentMethod()  ? Factory t?o ?úng payment method
    ?
IPaymentMethod (CreditCard/PayPal/etc)
    ?
PaymentResult (success/failed)
    ?
Update Order Status
    ?
Response to Client
```

## ?? Các Files ???c T?o

### Domain Layer
- `mtkpm.Domain/Enums/Business/PaymentMethodType.cs`

### Application Layer
- `mtkpm.Application/Common/Interfaces/Services/IPaymentMethod.cs`
- `mtkpm.Application/Common/Interfaces/Services/IPaymentFactory.cs`
- `mtkpm.Application/Common/Interfaces/Services/IPaymentService.cs`
- `mtkpm.Application/Features/Orders/Commands/ProcessPayment/ProcessPaymentCommand.cs`
- `mtkpm.Application/Features/Orders/Commands/ProcessPayment/ProcessPaymentCommandHandler.cs`
- `mtkpm.Application/Features/Orders/Commands/ProcessPayment/ProcessPaymentCommandValidator.cs`

### Infrastructure Layer
- `mtkpm.Infrastructure/Services/Payments/PaymentFactory.cs`
- `mtkpm.Infrastructure/Services/Payments/PaymentService.cs`
- `mtkpm.Infrastructure/Services/Payments/CreditCardPaymentService.cs`
- `mtkpm.Infrastructure/Services/Payments/BankTransferPaymentService.cs`
- `mtkpm.Infrastructure/Services/Payments/PayPalPaymentService.cs`
- `mtkpm.Infrastructure/Services/Payments/CODPaymentService.cs`

### Presentation Layer
- `mtkpm/Controllers/PaymentController.cs`

## ?? Test Endpoint

```bash
POST /api/payment/process
Content-Type: application/json

{
    "orderId": 1,
    "amount": 100.00,
    "paymentMethod": 1  // PaymentMethodType.CreditCard
}
```

---

**Factory Pattern ?ã ???c tri?n khai thành công!** ?
