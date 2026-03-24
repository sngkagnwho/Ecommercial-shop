namespace mtkpm.Admin.Models.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; } = true;
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class UserDetailViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<UserOrderViewModel> Orders { get; set; } = new();
    }

    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "";
    }

    public class CreateUserViewModel
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
