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

### Layered Architecture

1. **Presentation Layer** (`mtkpm.UI`, `mtkpm.Admin`)
   - Razor Pages
   - User interfaces
   - Request handling

2. **Application Layer** (`mtkpm.Application`)
   - Business logic
   - Use cases
   - DTOs

3. **Domain Layer** (`mtkpm.Domain`)
   - Core entities
   - Business rules
   - Interfaces

4. **Infrastructure Layer** (`mtkpm.Infrastructure`)
   - Database operations
   - External service integrations
   - Repository implementations

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
