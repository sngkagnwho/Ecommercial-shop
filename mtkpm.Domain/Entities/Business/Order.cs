using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using mtkpm.Domain.Entities.Base;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Domain.Entities.Business
{
    public class Order : BaseEntity
    {   
        // Relationships
        public int UserId { get; private set; }
        public virtual User? User { get; set; }      
        
        public string OrderNumber { get; private set; }       
        public DateTime OrderDate { get; private set; }     
        
        // Addresses
        public string ShippingAddress { get; private set; }
        public string? BillingAddress { get; private set; }      
    
        // Pricing
        public decimal SubTotal { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        
        // Status & Tracking
        public OrderStatus Status { get; private set; }
        
        // Payment
        public PaymentMethodType PaymentMethod { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }
        
        // Additional Info
        public string? Note { get; private set; }
        
        // Navigation Properties
        private readonly List<OrderItem> _orderItems = new();
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        
        protected Order() { }
        
        public Order(
            int userId, 
            string orderNumber, 
            string shippingAddress, 
            string? billingAddress,
            decimal subTotal,
            decimal shippingFee,
            decimal discount,
            PaymentMethodType paymentMethod,
            string? note = null)
        {
            UserId = userId;
            OrderNumber = orderNumber ?? throw new ArgumentNullException(nameof(orderNumber));
            OrderDate = DateTime.UtcNow;
            ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
            BillingAddress = billingAddress;
            
            SubTotal = subTotal >= 0 ? subTotal : throw new ArgumentException("SubTotal cannot be negative");
            ShippingFee = shippingFee >= 0 ? shippingFee : throw new ArgumentException("ShippingFee cannot be negative");
            Discount = discount >= 0 ? discount : throw new ArgumentException("Discount cannot be negative");
            TotalAmount = subTotal + shippingFee - discount;
            
            Status = OrderStatus.Pending;
            PaymentMethod = paymentMethod;
            IsPaid = false;
            Note = note;
        }
        
        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            _orderItems.Add(item);
            RecalculateTotals();
        }
        
        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
        
        public void MarkAsPaid()
        {
            IsPaid = true;
            PaidAt = DateTime.UtcNow;
        }
        
        public void UpdateShippingFee(decimal newShippingFee)
        {
            if (newShippingFee < 0)
                throw new ArgumentException("ShippingFee cannot be negative");
            
            ShippingFee = newShippingFee;
            RecalculateTotals();
        }
        
        public void ApplyDiscount(decimal discount)
        {
            if (discount < 0)
                throw new ArgumentException("Discount cannot be negative");
            
            Discount = discount;
            RecalculateTotals();
        }
        
        private void RecalculateTotals()
        {
            SubTotal = _orderItems.Sum(item => item.TotalPrice);
            TotalAmount = SubTotal + ShippingFee - Discount;
        }
    }
}
