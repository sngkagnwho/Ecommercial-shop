using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Configuration
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.DiscountType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.DiscountValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.MinimumOrderAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.MaximumDiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.BudgetLimit)
                .HasPrecision(18, 2);

            builder.Property(x => x.BudgetUsed)
                .HasPrecision(18, 2);

            builder.Property(x => x.ApplicableCategories)
                .HasMaxLength(500);

            builder.Property(x => x.ApplicableProducts)
                .HasMaxLength(500);

            builder.Property(x => x.AdminNotes)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.EndDate);

            // Relationships
            builder.HasMany(x => x.UsageHistories)
                .WithOne(x => x.Discount)
                .HasForeignKey(x => x.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
