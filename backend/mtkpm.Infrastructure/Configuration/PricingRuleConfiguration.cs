using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Configuration
{
    public class PricingRuleConfiguration : IEntityTypeConfiguration<PricingRule>
    {
        public void Configure(EntityTypeBuilder<PricingRule> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.RuleType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.RuleCondition)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.RuleValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.ApplicableProductIds)
                .HasMaxLength(1000);

            builder.Property(x => x.ApplicableCategoryIds)
                .HasMaxLength(1000);

            builder.Property(x => x.AdminNotes)
                .HasMaxLength(500);

            builder.HasIndex(x => x.RuleType);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.StartDate);
            builder.HasIndex(x => x.EndDate);
        }
    }
}
