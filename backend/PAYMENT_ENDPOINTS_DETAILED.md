# ?? MTKPM Payment API - Chi Ti?t Ph??ng Th?c Thanh Toán

**Base URL:** `http://localhost:5000/api/payment`

---

## ?? T?t C? Payment Endpoints

### 1. ?? L?y Danh Sách Ph??ng Th?c Thanh Toán
```
GET /api/payment/methods

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "code": "CreditCard",
      "name": "Th? Tín D?ng",
      "description": "Thanh toán b?ng th? tín d?ng (Visa, Mastercard, v.v.)",
      "icon": "??",
      "isActive": true,
      "fee": 0
    },
    {
      "code": "BankTransfer",
      "name": "Chuy?n Kho?n Ngân Hàng",
      "description": "Thanh toán b?ng chuy?n kho?n ngân hàng tr?c ti?p",
      "icon": "??",
      "isActive": true,
      "fee": 0
    },
    {
      "code": "COD",
      "name": "Thanh Toán Khi Nh?n Hàng (COD)",
      "description": "Thanh toán khi nh?n hàng, không c?n tr? ti?n tr??c",
      "icon": "??",
      "isActive": true,
      "fee": 0
    }
  ],
  "message": "Danh sách ph??ng th?c thanh toán"
}
```

---

### 2. ?? L?y Chi Ti?t Ph??ng Th?c Thanh Toán

#### Th? Tín D?ng
```
GET /api/payment/methods/CreditCard

Response: 200 OK
{
  "success": true,
  "data": {
    "code": "CreditCard",
    "name": "Th? Tín D?ng",
    "description": "Thanh toán an toàn b?ng th? tín d?ng",
    "processingTime": "T?c th?i",
    "requirements": [
      "S? th? tín d?ng",
      "Tên ch? th?",
      "Ngày h?t h?n",
      "CVV"
    ],
    "supportedCards": ["Visa", "Mastercard", "American Express"],
    "fee": 0,
    "minAmount": 10000,
    "maxAmount": 1000000000
  }
}
```

#### Chuy?n Kho?n Ngân Hàng
```
GET /api/payment/methods/BankTransfer

Response: 200 OK
{
  "success": true,
  "data": {
    "code": "BankTransfer",
    "name": "Chuy?n Kho?n Ngân Hàng",
    "description": "Chuy?n kho?n t? ngân hàng c?a b?n ??n ngân hàng c?a chúng tôi",
    "processingTime": "1-3 ngày làm vi?c",
    "requirements": [
      "Tên ngân hàng",
      "S? tài kho?n nh?n",
      "Mô t? chuy?n kho?n (Mã ??n hàng)"
    ],
    "supportedBanks": [
      "Vietcombank",
      "Techcombank",
      "BIDV",
      "VP Bank",
      "ACB",
      "Các ngân hàng khác"
    ],
    "fee": 0,
    "minAmount": 50000,
    "maxAmount": 5000000000
  }
}
```

#### COD (Thanh Toán Khi Nh?n Hàng)
```
GET /api/payment/methods/COD

Response: 200 OK
{
  "success": true,
  "data": {
    "code": "COD",
    "name": "Thanh Toán Khi Nh?n Hàng",
    "description": "B?n ch? thanh toán khi ?ã ki?m tra và nh?n hàng",
    "processingTime": "Khi giao hàng",
    "requirements": [
      "??a ch? giao hàng chính xác",
      "S? ?i?n tho?i liên h?"
    ],
    "availableAreas": [
      "Toàn thành ph? H? Chí Minh",
      "Toàn t?nh Bình D??ng",
      "Toàn t?nh ??ng Nai",
      "Các t?nh khác (phí giao hàng t?ng)"
    ],
    "fee": 0,
    "minAmount": 10000,
    "maxAmount": 10000000
  }
}
```

---

### 3. ?? X? Lý Thanh Toán

```
POST /api/payment/process
Content-Type: application/json
Authorization: Bearer {accessToken}

Request Body:
{
  "orderId": 1,
  "amount": 1000000,
  "paymentMethod": "CreditCard"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "transactionId": "TXN-123456",
    "orderId": 1,
    "amount": 1000000,
    "status": "Success",
    "message": "Thanh toán thành công"
  },
  "message": "Thanh toán ?ã ???c x? lý thành công"
}
```

**Các ph??ng th?c thanh toán (paymentMethod):**
- `CreditCard` - Th? tín d?ng
- `BankTransfer` - Chuy?n kho?n ngân hàng
- `COD` - Thanh toán khi nh?n hàng

---

### 4. ?? Ki?m Tra Tr?ng Thái Thanh Toán

```
GET /api/payment/status/{orderId}
Authorization: Bearer {accessToken}

Ví d?: GET /api/payment/status/1

Response: 200 OK
{
  "success": true,
  "data": {
    "orderId": 1,
    "status": "Pending",
    "paymentMethod": "CreditCard",
    "amount": 1000000,
    "transactionId": "TXN-1",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z",
    "message": "Thanh toán ?ang ch? xác nh?n"
  }
}
```

**Status Có Th?:**
- `Pending` - ?ang ch? xác nh?n
- `Success` - Thanh toán thành công
- `Failed` - Thanh toán th?t b?i
- `Refunded` - ?ã hoàn ti?n

---

### 5. ?? H??ng D?n Factory Pattern

```
GET /api/payment/guide

Response: 200 OK
{
  "guide": "Payment System - Factory Pattern..."
}
```

---

## ?? Quy Trình Thanh Toán

### 1?? L?y Danh Sách Ph??ng Th?c (Frontend)
```
GET /api/payment/methods
? Hi?n th? ra cho user ch?n
```

### 2?? User Ch?n Ph??ng Th?c
```
User ch?n: Th? Tín D?ng
```

### 3?? L?y Chi Ti?t Ph??ng Th?c (Frontend)
```
GET /api/payment/methods/CreditCard
? Hi?n th? requirements, min/max amount
```

### 4?? User Nh?p Thông Tin & T?o ??n Hàng
```
POST /api/orders
? Nh?n ???c orderId
```

### 5?? X? Lý Thanh Toán
```
POST /api/payment/process
{
  "orderId": 1,
  "amount": 1000000,
  "paymentMethod": "CreditCard"
}
? Th?c hi?n thanh toán
```

### 6?? Ki?m Tra Tr?ng Thái (Optional)
```
GET /api/payment/status/1
? Ki?m tra k?t qu? thanh toán
```

---

## ?? Ví D? Frontend Integration

### TypeScript/React Hook
```typescript
// L?y danh sách ph??ng th?c thanh toán
const getPaymentMethods = async () => {
  const response = await axios.get('/api/payment/methods');
  return response.data.data;
};

// L?y chi ti?t ph??ng th?c
const getPaymentMethodDetail = async (code: string) => {
  const response = await axios.get(`/api/payment/methods/${code}`);
  return response.data.data;
};

// X? lý thanh toán
const processPayment = async (orderId: number, amount: number, method: string) => {
  const response = await axios.post('/api/payment/process', {
    orderId,
    amount,
    paymentMethod: method
  }, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
  return response.data;
};

// Ki?m tra tr?ng thái
const getPaymentStatus = async (orderId: number) => {
  const response = await axios.get(`/api/payment/status/${orderId}`, {
    headers: {
      Authorization: `Bearer ${token}`
    }
  });
  return response.data.data;
};
```

---

## ? ??c ?i?m

### ?? Factory Pattern
- D? thêm ph??ng th?c thanh toán m?i
- Encapsulation logic thanh toán
- Flexible & maintainable

### ?? B?o M?t
- JWT Authentication required (except GET methods)
- Validate amount & orderId
- X? lý error an toàn

### ?? Tính Linh Ho?t
- Multiple payment methods
- Min/Max amounts
- Real-time status checking

---

## ?? Tóm T?t

| Endpoint | Method | M?c ?ích |
|----------|--------|---------|
| `/methods` | GET | L?y danh sách ph??ng th?c |
| `/methods/{code}` | GET | L?y chi ti?t ph??ng th?c |
| `/process` | POST | X? lý thanh toán |
| `/status/{orderId}` | GET | Ki?m tra tr?ng thái |
| `/guide` | GET | H??ng d?n |

---

**Last Updated:** 2024  
**Version:** 1.0
