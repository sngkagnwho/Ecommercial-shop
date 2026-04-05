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
        public bool IsLocked { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
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
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UpdateCurrentUserViewModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UserWithRolesViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsLocked { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class UpdateUserRoleViewModel
    {
        public int UserId { get; set; }
        public string RoleName { get; set; } = "User"; // "User", "Admin", "Moderator"
    }

    public class LockUserViewModel
    {
        public int UserId { get; set; }
        public bool IsLocked { get; set; }
    }

    public class UserStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsersThisMonth { get; set; }
        public int LockedUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
    }

    public class SearchUserViewModel
    {
        public string SearchTerm { get; set; } = "";
        public List<UserWithRolesViewModel> Results { get; set; } = new();
    }
}
