namespace mtkpm.Admin.Constants
{
    /// <summary>
    /// API endpoint constants - maps to backend controller routes
    /// </summary>
    public static class ApiEndpoints
    {
        // Auth endpoints
        public static class Auth
        {
            public const string Login = "/api/auth/login";
            public const string Register = "/api/auth/register";
            public const string RefreshToken = "/api/auth/refresh-token";
            public const string Logout = "/api/auth/logout";
            public const string ChangePassword = "/api/auth/change-password";
        }

        // Products endpoints
        public static class Products
        {
            public const string Base = "/api/products";
            public const string All = "/api/products/all";
            public const string GetById = "/api/products/{id}";
            public const string GetPaginated = "/api/products?pageIndex={pageIndex}&pageSize={pageSize}&categoryId={categoryId}&searchTerm={searchTerm}";
            public const string GetByCategory = "/api/products/category/{categoryId}";
            public const string Search = "/api/products/search";
            public const string Create = "/api/products";
            public const string Update = "/api/products/{id}";
            public const string Delete = "/api/products/{id}";
            public const string UpdateStock = "/api/products/{id}/stock";
        }

        // Orders endpoints
        public static class Orders
        {
            public const string Base = "/api/orders";
            public const string GetMyOrders = "/api/orders/my-orders";
            public const string GetById = "/api/orders/{id}";
            public const string GetByNumber = "/api/orders/number/{orderNumber}";
            public const string Create = "/api/orders";
            public const string UpdateStatus = "/api/orders/{id}/status";
            public const string MarkAsPaid = "/api/orders/{id}/mark-paid";
            public const string Cancel = "/api/orders/{id}/cancel";
        }

        // Users endpoints
        public static class Users
        {
            public const string Base = "/api/users";
            public const string GetMe = "/api/users/me";
            public const string UpdateMe = "/api/users/me";
            public const string Favourites = "/api/users/favourites";
            public const string AddFavourite = "/api/users/favourites";
            public const string RemoveFavourite = "/api/users/favourites/{productId}";
            public const string GetAll = "/api/users";
        }

        // Categories endpoints
        public static class Categories
        {
            public const string Base = "/api/categories";
            public const string GetAll = "/api/categories";
            public const string GetById = "/api/categories/{id}";
            public const string Create = "/api/categories";
            public const string Update = "/api/categories/{id}";
            public const string Delete = "/api/categories/{id}";
        }

        // Discount endpoints
        public static class Discount
        {
            public const string Calculate = "/api/discount/calculate";
            public const string Available = "/api/discount/available";
            public const string Guide = "/api/discount/guide";
        }

        // Payment endpoints
        public static class Payment
        {
            public const string Methods = "/api/payment/methods";
            public const string MethodDetail = "/api/payment/methods/{code}";
            public const string Process = "/api/payment/process";
            public const string Status = "/api/payment/status/{orderId}";
        }

        // Notification endpoints
        public static class Notifications
        {
            public const string Subscribers = "/api/notification/subscribers";
            public const string TestOrderCreated = "/api/notification/test/order-created";
            public const string TestPaymentCompleted = "/api/notification/test/payment-completed";
            public const string TestOrderShipped = "/api/notification/test/order-shipped";
            public const string TestPaymentFailed = "/api/notification/test/payment-failed";
            public const string TestOrderCancelled = "/api/notification/test/order-cancelled";
            public const string Guide = "/api/notification/guide";
            public const string GetAll = "/api/notification/subscribers";
            public const string Send = "/api/notification/test/order-created";
        }

        // Discounts endpoints (plural for services)
        public static class Discounts
        {
            public const string GetAll = "/api/discount/available";
            public const string Calculate = "/api/discount/calculate";
            public const string Guide = "/api/discount/guide";
            public const string Base = "/api/discount/calculate";
        }

        // Payments endpoints (plural for services)
        public static class Payments
        {
            public const string GetHistory = "/api/payment/status/{orderId}";
            public const string Process = "/api/payment/process";
            public const string Methods = "/api/payment/methods";
            public const string MethodDetail = "/api/payment/methods/{code}";
            public const string Status = "/api/payment/status/{orderId}";
            public const string GetById = "/api/payment/methods";
        }

        // Pricing endpoints
        public static class Pricing
        {
            public const string Calculate = "/api/pricing/calculate";
            public const string Strategies = "/api/pricing/strategies";
        }

        // Cart endpoints
        public static class Cart
        {
            public const string Base = "/api/cart";
            public const string Get = "/api/cart";
            public const string Add = "/api/cart";
            public const string Update = "/api/cart/{cartItemId}";
            public const string Delete = "/api/cart/{cartItemId}";
            public const string DeleteAll = "/api/cart";
            public const string Count = "/api/cart/count";
        }
    }
}
