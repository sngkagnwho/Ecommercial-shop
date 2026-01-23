using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using mtkpm.Domain.Entities.Base;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Domain.Enums.Business;

namespace mtkpm.Domain.Entities.Business
{
    /// <summary>
    /// Order Aggregate Root - represents a customer order
    /// Following Clean Architecture and DDD principles
    /// </summary>
    public class Order : BaseEntity
    {
        #region Properties - Data/State
        
        // Relationships
        public int UserId { get; private set; }
        public virtual User? User { get; set; }
        
        // Order Information
        [MaxLength(50)]
        public string OrderNumber { get; private set; }
        
        public DateTime OrderDate { get; private set; }
        
        // Address Information (Snapshot at order time)
        [MaxLength(500)]
        [Required]
        public string ShippingAddress { get; private set; }
        
        [MaxLength(500)]
        public string? BillingAddress { get; private set; }
        
        // Financial Information
        public decimal SubTotal { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        
        // Status & Tracking
        public OrderStatus Status { get; private set; }
        public bool IsPaid { get; private set; }
        public DateTime? PaidAt { get; private set; }
        
        [MaxLength(100)]
        public string? PaymentMethod { get; private set; }
        
        [MaxLength(1000)]
        public string? Note { get; private set; }
        
        // Navigation Properties
        private readonly List<OrderItem> _orderItems = new();
        public virtual IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        
        #endregion
        
        #region Constructors
        
        // EF Core constructor
        protected Order() 
        {
            OrderNumber = string.Empty;
            ShippingAddress = string.Empty;
        }
        
        // Domain constructor - ensures valid state
        public Order(
            int userId, 
            string shippingAddress, 
            string? billingAddress = null,
            string? note = null)
        {
            // Validation
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));
            
            if (string.IsNullOrWhiteSpace(shippingAddress))
                throw new ArgumentException("Shipping address is required", nameof(shippingAddress));
            
            // Initialize
            UserId = userId;
            OrderNumber = GenerateOrderNumber();
            OrderDate = DateTime.UtcNow;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress ?? shippingAddress;
            Note = note;
            Status = OrderStatus.Pending;
            IsPaid = false;
            
            // Set audit fields
            SetCreated(userId);
        }
        
        #endregion
        
        #region Business Rules & Invariants
        
        /// <summary>
        /// Add item to order - ensures business rules
        /// </summary>
        public void AddItem(int productId, string productName, int quantity, decimal price)
        {
            // Business rule: Can only add items to pending orders
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot add items to non-pending order");
            
            // Business rule: Validate item
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
            
            if (price < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));
            
            // Add item
            var orderItem = new OrderItem(Id, productId, productName, quantity, price);
            _orderItems.Add(orderItem);
            
            // Recalculate totals
            RecalculateTotals();
        }
        
        /// <summary>
        /// Remove item from order
        /// </summary>
        public void RemoveItem(int orderItemId)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot remove items from non-pending order");
            
            var item = _orderItems.FirstOrDefault(x => x.Id == orderItemId);
            if (item != null)
            {
                _orderItems.Remove(item);
                RecalculateTotals();
            }
        }
        
        /// <summary>
        /// Set shipping fee
        /// </summary>
        public void SetShippingFee(decimal fee)
        {
            if (fee < 0)
                throw new ArgumentException("Shipping fee cannot be negative", nameof(fee));
            
            ShippingFee = fee;
            RecalculateTotals();
        }
        
        /// <summary>
        /// Apply discount
        /// </summary>
        public void ApplyDiscount(decimal discountAmount)
        {
            if (discountAmount < 0)
                throw new ArgumentException("Discount cannot be negative", nameof(discountAmount));
            
            if (discountAmount > SubTotal)
                throw new ArgumentException("Discount cannot exceed subtotal", nameof(discountAmount));
            
            Discount = discountAmount;
            RecalculateTotals();
        }
        
        /// <summary>
        /// Update order status - ensures valid transitions
        /// </summary>
        public void UpdateStatus(OrderStatus newStatus)
        {
            // Business rule: Validate status transitions
            if (!IsValidStatusTransition(Status, newStatus))
                throw new InvalidOperationException($"Cannot transition from {Status} to {newStatus}");
            
            Status = newStatus;
        }
        
        /// <summary>
        /// Mark order as paid
        /// </summary>
        public void MarkAsPaid(string paymentMethod)
        {
            if (IsPaid)
                throw new InvalidOperationException("Order is already paid");
            
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method is required", nameof(paymentMethod));
            
            IsPaid = true;
            PaidAt = DateTime.UtcNow;
            PaymentMethod = paymentMethod;
        }
        
        /// <summary>
        /// Cancel order
        /// </summary>
        public void Cancel()
        {
            // Business rule: Cannot cancel completed or delivered orders
            if (Status == OrderStatus.Completed || Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot cancel completed or delivered orders");
            
            Status = OrderStatus.Cancelled;
        }
        
        #endregion
        
        #region Private Helper Methods
        
        private void RecalculateTotals()
        {
            SubTotal = _orderItems.Sum(item => item.TotalPrice);
            TotalAmount = SubTotal + ShippingFee - Discount;
            
            // Business rule: Total cannot be negative
            if (TotalAmount < 0)
                TotalAmount = 0;
        }
        
        private string GenerateOrderNumber()
        {
            return $"ORD{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }
        
        private bool IsValidStatusTransition(OrderStatus from, OrderStatus to)
        {
            // Define valid transitions
            return (from, to) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Processing, OrderStatus.Shipping) => true,
                (OrderStatus.Processing, OrderStatus.Cancelled) => true,
                (OrderStatus.Shipping, OrderStatus.Delivered) => true,
                (OrderStatus.Delivered, OrderStatus.Completed) => true,
                (OrderStatus.Delivered, OrderStatus.Returned) => true,
                _ => false
            };
        }
        
        #endregion
    }
}
