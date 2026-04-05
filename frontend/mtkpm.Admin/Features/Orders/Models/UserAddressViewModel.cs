namespace mtkpm.Admin.Features.Orders.Models
{
    /// <summary>
    /// User address view model for displaying saved addresses
    /// Maps to backend UserAddressDto
    /// </summary>
    public class UserAddressViewModel
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsDefault { get; set; }

        /// <summary>
        /// Full formatted address for display
        /// </summary>
        public string GetFullAddress()
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(ReceiverName)) parts.Add($"Người nhận: {ReceiverName}");
            if (!string.IsNullOrEmpty(PhoneNumber)) parts.Add($"SĐT: {PhoneNumber}");
            if (!string.IsNullOrEmpty(Street)) parts.Add(Street);
            if (!string.IsNullOrEmpty(Ward)) parts.Add(Ward);
            if (!string.IsNullOrEmpty(District)) parts.Add(District);
            if (!string.IsNullOrEmpty(City)) parts.Add(City);
            if (!string.IsNullOrEmpty(PostalCode)) parts.Add($"Mã: {PostalCode}");
            return string.Join(", ", parts);
        }
    }

    /// <summary>
    /// Create address request model
    /// </summary>
    public class CreateUserAddressViewModel
    {
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = "Việt Nam";
        public string Label { get; set; } = "Khác";
        public bool IsDefault { get; set; } = false;
    }

    /// <summary>
    /// Update address request model
    /// </summary>
    public class UpdateUserAddressViewModel
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = "Việt Nam";
        public string Label { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
