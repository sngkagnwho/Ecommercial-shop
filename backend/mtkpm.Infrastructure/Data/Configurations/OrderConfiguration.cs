using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.BillingAddress)
                .HasMaxLength(500);

            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ShippingFee)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Discount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.Note)
                .HasMaxLength(500);

            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();
            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.OrderDate);
        }
    }
}
