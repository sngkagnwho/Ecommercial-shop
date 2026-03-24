# ?? MTKPM E-Commerce API - T?t C? Endpoint

**Base URL:** `http://localhost:5000`

---

## ?? M?c L?c
1. [Authentication (Auth)](#authentication-auth)
2. [Products](#products)
3. [Categories](#categories)
4. [Cart](#cart)
5. [Orders](#orders)
6. [Payment](#payment)
7. [Discount](#discount)
8. [Pricing](#pricing)
9. [Notification](#notification)
10. [Users](#users)

---

## Authentication (Auth)

### 1. ??ng Ký Tài Kho?n
```
POST /api/auth/register
Content-Type: application/json

{
  "userName": "string",
  "email": "string",
  "password": "string",
  "confirmPassword": "string",
  "phoneNumber": "string"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "userId": 0,
    "userName": "string",
    "email": "string",
    "accessToken": "string",
    "refreshToken": "string",
    "expiresIn": 0
  },
  "message": "string"
}
```

### 2. ??ng Nh?p
```
POST /api/auth/login
Content-Type: application/json

{
  "userNameOrEmail": "string",
  "password": "string",
  "rememberMe": true
}

Response: 200 OK
{
  "success": true,
  "data": {
    "userId": 0,
    "userName": "string",
    "email": "string",
    "accessToken": "string",
    "refreshToken": "string",
    "expiresIn": 0
  },
  "message": "string"
}
```

### 3. Làm M?i Access Token
```
POST /api/auth/refresh-token
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "accessToken": "string",
  "refreshToken": "string"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "userId": 0,
    "userName": "string",
    "email": "string",
    "accessToken": "string",
    "refreshToken": "string",
    "expiresIn": 0
  },
  "message": "string"
}
```

### 4. ??ng Xu?t
```
POST /api/auth/logout
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "accessToken": "string",
  "refreshToken": "string"
}

Response: 200 OK
{
  "message": "??ng xu?t thành công."
}
```

### 5. ??i M?t Kh?u
```
POST /api/auth/change-password
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "currentPassword": "string",
  "newPassword": "string",
  "confirmNewPassword": "string"
}

Response: 200 OK
{
  "message": "?ã c?p nh?t m?t kh?u thành công"
}
```

---

## Products

### 1. L?y Danh Sách S?n Ph?m (Phân Trang)
```
GET /api/products?pageIndex=1&pageSize=10&categoryId=1&searchTerm=iPhone

Response: 200 OK
{
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "name": "string",
        "description": "string",
        "price": 0,
        "stockQuantity": 0,
        "imageUrl": "string",
        "categoryId": 0,
        "categoryName": "string",
        "createdAt": "2024-01-01T00:00:00Z"
      }
    ],
    "totalCount": 0,
    "pageIndex": 1,
    "pageSize": 10,
    "totalPages": 0
  },
  "message": "string"
}
```

### 2. L?y T?t C? S?n Ph?m
```
GET /api/products/all

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "description": "string",
      "price": 0,
      "stockQuantity": 0,
      "imageUrl": "string",
      "categoryId": 0,
      "categoryName": "string",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 3. L?y S?n Ph?m Theo ID
```
GET /api/products/{id}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "price": 0,
    "stockQuantity": 0,
    "imageUrl": "string",
    "categoryId": 0,
    "categoryName": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "string"
}
```

### 4. L?y S?n Ph?m Theo Danh M?c
```
GET /api/products/category/{categoryId}

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "description": "string",
      "price": 0,
      "stockQuantity": 0,
      "imageUrl": "string",
      "categoryId": 0,
      "categoryName": "string",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 5. Tìm Ki?m S?n Ph?m
```
GET /api/products/search?searchTerm=iPhone

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "description": "string",
      "price": 0,
      "stockQuantity": 0,
      "imageUrl": "string",
      "categoryId": 0,
      "categoryName": "string",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 6. T?o S?n Ph?m (Admin Only)
```
POST /api/products
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "name": "string",
  "description": "string",
  "price": 0,
  "stockQuantity": 0,
  "imageUrl": "string",
  "categoryId": 0
}

Response: 201 Created
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "price": 0,
    "stockQuantity": 0,
    "imageUrl": "string",
    "categoryId": 0,
    "categoryName": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "T?o s?n ph?m thành công"
}
```

### 7. C?p Nh?t S?n Ph?m (Admin Only)
```
PUT /api/products/{id}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "name": "string",
  "description": "string",
  "price": 0,
  "stockQuantity": 0,
  "imageUrl": "string",
  "categoryId": 0
}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "price": 0,
    "stockQuantity": 0,
    "imageUrl": "string",
    "categoryId": 0,
    "categoryName": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "C?p nh?t s?n ph?m thành công"
}
```

### 8. Xóa S?n Ph?m (Admin Only)
```
DELETE /api/products/{id}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "Xóa s?n ph?m thành công"
}
```

### 9. C?p Nh?t S? L??ng T?n Kho (Admin Only)
```
PATCH /api/products/{id}/stock
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "quantity": 100
}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "C?p nh?t t?n kho thành công"
}
```

---

## Categories

### 1. L?y T?t C? Danh M?c
```
GET /api/categories

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "string",
      "description": "string",
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 2. L?y Danh M?c Theo ID
```
GET /api/categories/{id}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "string"
}
```

### 3. T?o Danh M?c (Admin Only)
```
POST /api/categories
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "name": "string",
  "description": "string"
}

Response: 201 Created
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "T?o danh m?c thành công"
}
```

### 4. C?p Nh?t Danh M?c (Admin Only)
```
PUT /api/categories/{id}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "name": "string",
  "description": "string"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "C?p nh?t danh m?c thành công"
}
```

### 5. Xóa Danh M?c (Admin Only)
```
DELETE /api/categories/{id}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "Xóa danh m?c thành công"
}
```

---

## Cart

### 1. L?y Gi? Hàng
```
GET /api/cart
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "userId": 1,
    "cartItems": [
      {
        "id": 1,
        "productId": 1,
        "productName": "string",
        "quantity": 2,
        "price": 0,
        "totalPrice": 0,
        "imageUrl": "string"
      }
    ],
    "totalItems": 0,
    "totalPrice": 0,
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "string"
}
```

### 2. L?y S? L??ng S?n Ph?m Trong Gi?
```
GET /api/cart/count
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": 5,
  "message": "string"
}
```

### 3. Thêm S?n Ph?m Vào Gi? Hàng
```
POST /api/cart
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "productId": 1,
  "quantity": 2
}

Response: 201 Created
{
  "success": true,
  "data": {
    "id": 1,
    "productId": 1,
    "productName": "string",
    "quantity": 2,
    "price": 0,
    "totalPrice": 0,
    "imageUrl": "string"
  },
  "message": "Thêm vào gi? hàng thành công"
}
```

### 4. C?p Nh?t S? L??ng S?n Ph?m Trong Gi?
```
PUT /api/cart/{cartItemId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "quantity": 5
}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "productId": 1,
    "productName": "string",
    "quantity": 5,
    "price": 0,
    "totalPrice": 0,
    "imageUrl": "string"
  },
  "message": "C?p nh?t gi? hàng thành công"
}
```

### 5. Xóa S?n Ph?m Kh?i Gi? Hàng
```
DELETE /api/cart/{cartItemId}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "Xóa kh?i gi? hàng thành công"
}
```

### 6. Xóa Toàn B? Gi? Hàng
```
DELETE /api/cart
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "?ã xóa toàn b? gi? hàng"
}
```

---

## Orders

### 1. L?y Danh Sách ??n Hàng C?a User
```
GET /api/orders/my-orders
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "id": 1,
      "orderNumber": "ORD-001",
      "userId": 1,
      "totalAmount": 0,
      "status": "Pending",
      "paymentMethod": "CreditCard",
      "shippingAddress": "string",
      "billingAddress": "string",
      "note": "string",
      "orderItems": [
        {
          "id": 1,
          "orderId": 1,
          "productId": 1,
          "productName": "string",
          "quantity": 1,
          "price": 0,
          "totalPrice": 0
        }
      ],
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 2. L?y ??n Hàng Theo ID
```
GET /api/orders/{id}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "orderNumber": "ORD-001",
    "userId": 1,
    "totalAmount": 0,
    "status": "Pending",
    "paymentMethod": "CreditCard",
    "shippingAddress": "string",
    "billingAddress": "string",
    "note": "string",
    "orderItems": [
      {
        "id": 1,
        "orderId": 1,
        "productId": 1,
        "productName": "string",
        "quantity": 1,
        "price": 0,
        "totalPrice": 0
      }
    ],
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "string"
}
```

### 3. L?y ??n Hàng Theo S? ??n Hàng
```
GET /api/orders/number/{orderNumber}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "orderNumber": "ORD-001",
    "userId": 1,
    "totalAmount": 0,
    "status": "Pending",
    "paymentMethod": "CreditCard",
    "shippingAddress": "string",
    "billingAddress": "string",
    "note": "string",
    "orderItems": [
      {
        "id": 1,
        "orderId": 1,
        "productId": 1,
        "productName": "string",
        "quantity": 1,
        "price": 0,
        "totalPrice": 0
      }
    ],
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "string"
}
```

### 4. T?o ??n Hàng
```
POST /api/orders
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "shippingAddress": "string",
  "billingAddress": "string",
  "paymentMethod": "CreditCard",
  "note": "string",
  "orderItems": [
    {
      "productId": 1,
      "quantity": 2
    }
  ]
}

Response: 201 Created
{
  "success": true,
  "data": {
    "id": 1,
    "orderNumber": "ORD-001",
    "userId": 1,
    "totalAmount": 0,
    "status": "Pending",
    "paymentMethod": "CreditCard",
    "shippingAddress": "string",
    "billingAddress": "string",
    "note": "string",
    "orderItems": [
      {
        "id": 1,
        "orderId": 1,
        "productId": 1,
        "productName": "string",
        "quantity": 2,
        "price": 0,
        "totalPrice": 0
      }
    ],
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "??t hàng thành công"
}
```

### 5. H?y ??n Hàng
```
POST /api/orders/{id}/cancel
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "H?y ??n hàng thành công"
}
```

### 6. C?p Nh?t Tr?ng Thái ??n Hàng (Admin Only)
```
PATCH /api/orders/{id}/status
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "status": "Confirmed"
}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "C?p nh?t tr?ng thái thành công"
}
```

### 7. ?ánh D?u ??n Hàng ?ã Thanh Toán (Admin Only)
```
POST /api/orders/{id}/mark-paid
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "?ánh d?u ?ã thanh toán"
}
```

---

## Payment

### 1. X? Lý Thanh Toán
```
POST /api/payment/process
Content-Type: application/json
Authorization: Bearer {accessToken}

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

**Các lo?i paymentMethod:**
- `CreditCard` - Th? tín d?ng
- `BankTransfer` - Chuy?n kho?n ngân hàng
- `COD` - Thanh toán khi nh?n hàng

---

## Discount

### 1. Tính Giá Gi? Hàng Sau Khi Áp D?ng Chi?t Kh?u
```
POST /api/discount/calculate
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "discountCodes": ["percentage_10", "free_shipping"]
}

Response: 200 OK
{
  "success": true,
  "data": {
    "originalPrice": 1000000,
    "discountAmount": 100000,
    "finalPrice": 900000,
    "appliedDiscounts": [
      {
        "code": "percentage_10",
        "name": "Gi?m 10%",
        "amount": 100000
      },
      {
        "code": "free_shipping",
        "name": "Mi?n phí v?n chuy?n",
        "amount": 50000
      }
    ]
  },
  "message": "Tính chi?t kh?u thành công"
}
```

### 2. L?y Danh Sách Mã Chi?t Kh?u Có S?n
```
GET /api/discount/available

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "code": "percentage_10",
      "name": "Gi?m 10%",
      "description": "Gi?m 10% trên giá hàng",
      "example": "percentage_10"
    },
    {
      "code": "fixed_100000",
      "name": "Gi?m 100K",
      "description": "Gi?m 100.000 ? c? ??nh",
      "example": "fixed_100000"
    },
    {
      "code": "free_shipping",
      "name": "Mi?n phí v?n chuy?n",
      "description": "Mi?n phí v?n chuy?n (ti?t ki?m 50.000 ?)",
      "example": "free_shipping"
    },
    {
      "code": "loyalty_points_50",
      "name": "50 ?i?m thành viên",
      "description": "S? d?ng 50 ?i?m thành viên (50.000 ?)",
      "example": "loyalty_points_50"
    },
    {
      "code": "bundle_3_15",
      "name": "Chi?t kh?u combo",
      "description": "Mua 3+ s?n ph?m ???c gi?m 15%",
      "example": "bundle_3_15"
    }
  ],
  "message": "string"
}
```

### 3. H??ng D?n S? D?ng Decorator Pattern
```
GET /api/discount/guide

Response: 200 OK
{
  "guide": "Decorator Pattern - H? th?ng Chi?t kh?u..."
}
```

---

## Pricing

### 1. Tính Giá S?n Ph?m S? D?ng Strategy Pattern
```
POST /api/pricing/calculate
Content-Type: application/json

{
  "productId": 1,
  "quantity": 10,
  "pricingStrategy": "bulk"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "productId": 1,
    "originalPrice": 100000,
    "quantity": 10,
    "appliedStrategy": "bulk",
    "finalPrice": 90000,
    "discountPercentage": 10,
    "totalPrice": 900000
  },
  "message": "Tính giá thành công"
}
```

### 2. L?y Danh Sách Chi?n L??c ??nh Giá
```
GET /api/pricing/strategies

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "name": "regular",
      "displayName": "Giá th??ng",
      "description": "Giá bán th??ng không có chi?t kh?u"
    },
    {
      "name": "bulk",
      "displayName": "Chi?t kh?u s? l??ng",
      "description": "Gi?m giá khi mua 10+ s?n ph?m (gi?m 10%)"
    },
    {
      "name": "seasonal",
      "displayName": "Giá mùa v?",
      "description": "Giá ??c bi?t theo mùa/d?p l?"
    },
    {
      "name": "vip",
      "displayName": "Giá thành viên VIP",
      "description": "Giá ??c bi?t cho thành viên VIP"
    }
  ],
  "message": "string"
}
```

---

## Notification

### 1. L?y Danh Sách Observer ?ang ??ng Ký
```
GET /api/notification/subscribers
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "totalSubscribers": 3,
    "subscribers": [
      "EmailNotificationService",
      "SMSNotificationService",
      "PushNotificationService"
    ],
    "message": "3 observer ?ang l?ng nghe các s? ki?n"
  },
  "message": "string"
}
```

### 2. Test - S? Ki?n T?o ??n Hàng
```
POST /api/notification/test/order-created
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "message": "S? ki?n t?o ??n hàng ?ã ???c công b? t?i t?t c? observers"
  },
  "message": "string"
}
```

### 3. Test - S? Ki?n Thanh Toán Hoàn Thành
```
POST /api/notification/test/payment-completed
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "message": "S? ki?n thanh toán hoàn thành ?ã ???c công b? t?i t?t c? observers"
  },
  "message": "string"
}
```

### 4. Test - S? Ki?n ??n Hàng ???c G?i ?i
```
POST /api/notification/test/order-shipped
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "message": "S? ki?n ??n hàng ???c g?i ?i ?ã ???c công b? t?i t?t c? observers"
  },
  "message": "string"
}
```

### 5. Test - S? Ki?n Thanh Toán Th?t B?i
```
POST /api/notification/test/payment-failed
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "message": "S? ki?n thanh toán th?t b?i ?ã ???c công b? t?i t?t c? observers"
  },
  "message": "string"
}
```

### 6. Test - S? Ki?n ??n Hàng B? H?y
```
POST /api/notification/test/order-cancelled
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "message": "S? ki?n ??n hàng b? h?y ?ã ???c công b? t?i t?t c? observers"
  },
  "message": "string"
}
```

### 7. H??ng D?n Observer Pattern
```
GET /api/notification/guide

Response: 200 OK
{
  "guide": "Observer Pattern - H? th?ng Thông báo..."
}
```

---

## Users

### 1. L?y Thông Tin User Hi?n T?i
```
GET /api/users/me
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "userName": "string",
    "email": "string",
    "roles": ["User"]
  },
  "message": "string"
}
```

### 2. C?p Nh?t Thông Tin User
```
PUT /api/users/me
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "userName": "string",
  "email": "string",
  "phoneNumber": "string"
}

Response: 200 OK
{
  "success": true,
  "data": {
    "id": 1,
    "userName": "string",
    "email": "string",
    "phoneNumber": "string",
    "emailConfirmed": true,
    "createdAt": "2024-01-01T00:00:00Z"
  },
  "message": "C?p nh?t thông tin thành công"
}
```

### 3. L?y Danh Sách S?n Ph?m Yêu Thích
```
GET /api/users/favourites
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": [
    {
      "productId": 1,
      "productName": "string",
      "imageUrl": "string",
      "price": 0,
      "addedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "message": "string"
}
```

### 4. Thêm S?n Ph?m Vào Yêu Thích
```
POST /api/users/favourites
Content-Type: application/json
Authorization: Bearer {accessToken}

{
  "productId": 1
}

Response: 201 Created
{
  "success": true,
  "data": {
    "productId": 1,
    "productName": "string",
    "imageUrl": "string",
    "price": 0,
    "addedAt": "2024-01-01T00:00:00Z"
  },
  "message": "Thêm vào yêu thích thành công"
}
```

### 5. Xóa S?n Ph?m Kh?i Yêu Thích
```
DELETE /api/users/favourites/{productId}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": true,
  "message": "?ã xóa kh?i yêu thích"
}
```

### 6. L?y Danh Sách T?t C? Ng??i Dùng (Admin Only)
```
GET /api/users?pageIndex=1&pageSize=20
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "userName": "string",
        "email": "string",
        "phoneNumber": "string",
        "emailConfirmed": true,
        "createdAt": "2024-01-01T00:00:00Z",
        "roles": ["User"]
      }
    ],
    "totalCount": 0,
    "pageIndex": 1,
    "pageSize": 20,
    "totalPages": 0
  },
  "message": "string"
}
```

---

## ?? Ghi Chú Quan Tr?ng

### Authentication
- T?t c? endpoint ???c ?ánh d?u `[Authorize]` yêu c?u token trong header:
  ```
  Authorization: Bearer {accessToken}
  ```

### Roles
- `[Authorize(Roles = "Admin")]` - Ch? Admin m?i có th? truy c?p
- `[AllowAnonymous]` - Không c?n authorization

### Response Format
T?t c? response theo format:
```json
{
  "success": true,
  "data": {...},
  "message": "string"
}
```

### Status Codes
- `200 OK` - Thành công (GET, PUT, PATCH, POST v?i d? li?u)
- `201 Created` - T?o thành công (POST)
- `400 Bad Request` - Request không h?p l?
- `401 Unauthorized` - Không có authorization
- `403 Forbidden` - Không có quy?n truy c?p
- `404 Not Found` - Không tìm th?y resource
- `500 Internal Server Error` - L?i server

---

## ?? Test Endpoint Ngay

**Swagger UI:** `http://localhost:5000`

---

## ?? H? Tr?
Liên h?: support@mtkpm.com
