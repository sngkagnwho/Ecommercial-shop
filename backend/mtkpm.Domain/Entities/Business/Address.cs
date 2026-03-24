using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtkpm.Domain.Entities.Business
{
    // Value Object - embedded vào Order/User
    public class Address
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string Ward { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string PhoneNumber { get; private set; }
        public string ReceiverName { get; private set; }

        // Constructor for immutability
        public Address(string street, string city, string district, string ward,
                      string postalCode, string country, string phoneNumber, string receiverName)
        {
            Street = street;
            City = city;
            District = district;
            Ward = ward;
            PostalCode = postalCode;
            Country = country;
            PhoneNumber = phoneNumber;
            ReceiverName = receiverName;
        }

     

       
    }
}