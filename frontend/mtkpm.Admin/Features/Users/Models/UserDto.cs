namespace mtkpm.Admin.Features.Users.Models
{
    /// <summary>
    /// User list item DTO
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// User detail DTO
    /// </summary>
    public class UserDetailDto : UserDto
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public List<string>? Roles { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
