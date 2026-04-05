using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Configuration
{
    public class DiscountUsageHistoryConfiguration : IEntityTypeConfiguration<DiscountUsageHistory>
    {
        public void Configure(EntityTypeBuilder<DiscountUsageHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.HasIndex(x => x.DiscountId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.UsedAt);

            // Relationships
            builder.HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
