using mtkpm.Domain.Entities.Base;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Domain.Entities.Business
{
    public class UserAddress : BaseEntity
    {
        public int UserId { get; private set; }
        public virtual User? User { get; set; }

        public string ReceiverName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Street { get; private set; }
        public string District { get; private set; }
        public string Ward { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }

        public bool IsDefault { get; private set; }
        public string Label { get; private set; } // "Nhà riêng", "Công ty", etc.

        protected UserAddress() { }

        public UserAddress(
            int userId,
            string receiverName,
            string phoneNumber,
            string street,
            string district,
            string ward,
            string city,
            string postalCode,
            string country,
            string label = "Khác",
            bool isDefault = false)
        {
            UserId = userId;
            ReceiverName = receiverName ?? throw new ArgumentNullException(nameof(receiverName));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            District = district ?? throw new ArgumentNullException(nameof(district));
            Ward = ward ?? throw new ArgumentNullException(nameof(ward));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Label = label;
            IsDefault = isDefault;
        }

        public void Update(
            string receiverName,
            string phoneNumber,
            string street,
            string district,
            string ward,
            string city,
            string postalCode,
            string country,
            string label)
        {
            ReceiverName = receiverName ?? throw new ArgumentNullException(nameof(receiverName));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            District = district ?? throw new ArgumentNullException(nameof(district));
            Ward = ward ?? throw new ArgumentNullException(nameof(ward));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Label = label;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void UnsetDefault()
        {
            IsDefault = false;
        }

        public Address ToAddress()
        {
            return new Address(
                Street,
                City,
                District,
                Ward,
                PostalCode,
                Country,
                PhoneNumber,
                ReceiverName);
        }
    }
}
