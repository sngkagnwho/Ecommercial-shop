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
            public const string ValidateToken = "/api/auth/validate";
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
            public const string MarkAsPaid = "/api/orders/{id}/mark-as-paid";
            public const string Cancel = "/api/orders/{id}/cancel";
        }

        // Users endpoints
        public static class Users
        {
            public const string Base = "/api/users";
            public const string GetById = "/api/users/{id}";
            public const string GetAll = "/api/users/all";
            public const string GetProfile = "/api/users/profile";
            public const string Create = "/api/users";
            public const string Update = "/api/users/{id}";
            public const string Delete = "/api/users/{id}";
            public const string ChangePassword = "/api/users/{id}/change-password";
        }

        // Categories endpoints
        public static class Categories
        {
            public const string Base = "/api/categories";
            public const string GetAll = "/api/categories/all";
            public const string GetById = "/api/categories/{id}";
            public const string Create = "/api/categories";
            public const string Update = "/api/categories/{id}";
            public const string Delete = "/api/categories/{id}";
        }

        // Discounts endpoints
        public static class Discounts
        {
            public const string Base = "/api/discounts";
            public const string GetAll = "/api/discounts/all";
            public const string GetById = "/api/discounts/{id}";
            public const string Create = "/api/discounts";
            public const string Update = "/api/discounts/{id}";
            public const string Delete = "/api/discounts/{id}";
            public const string Apply = "/api/discounts/apply";
        }

        // Payments endpoints
        public static class Payments
        {
            public const string Base = "/api/payments";
            public const string GetHistory = "/api/payments/history";
            public const string GetById = "/api/payments/{id}";
            public const string Process = "/api/payments/process";
            public const string Refund = "/api/payments/{id}/refund";
        }

        // Notifications endpoints
        public static class Notifications
        {
            public const string Base = "/api/notifications";
            public const string GetAll = "/api/notifications";
            public const string GetById = "/api/notifications/{id}";
            public const string Send = "/api/notifications/send";
            public const string Delete = "/api/notifications/{id}";
        }

        // Pricing endpoints
        public static class Pricing
        {
            public const string Base = "/api/pricing";
            public const string GetRules = "/api/pricing/rules";
            public const string CalculatePrice = "/api/pricing/calculate";
        }

        // Cart endpoints
        public static class Cart
        {
            public const string Base = "/api/cart";
            public const string GetMyCart = "/api/cart/my-cart";
            public const string AddItem = "/api/cart/add";
            public const string RemoveItem = "/api/cart/remove/{itemId}";
            public const string UpdateQuantity = "/api/cart/{itemId}/quantity";
            public const string Clear = "/api/cart/clear";
        }
    }
}
