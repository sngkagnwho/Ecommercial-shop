using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Configuration
{
    public class PaymentMethodConfigConfiguration : IEntityTypeConfiguration<PaymentMethodConfig>
    {
        public void Configure(EntityTypeBuilder<PaymentMethodConfig> builder)
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

            builder.Property(x => x.Icon)
                .HasMaxLength(50);

            builder.Property(x => x.ProcessingTime)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.TransactionFeePercentage)
                .HasPrecision(5, 2);

            builder.Property(x => x.TransactionFeeFixed)
                .HasPrecision(18, 2);

            builder.Property(x => x.MinAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.MaxAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Requirements)
                .HasMaxLength(1000);

            builder.Property(x => x.SupportedProviders)
                .HasMaxLength(500);

            builder.Property(x => x.SupportedAreas)
                .HasMaxLength(500);

            builder.Property(x => x.AdminNotes)
                .HasMaxLength(500);

            builder.Property(x => x.WebhookUrl)
                .HasMaxLength(500);

            builder.Property(x => x.ApiKey)
                .HasMaxLength(500);

            builder.Property(x => x.Configuration)
                .HasMaxLength(2000);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.DisplayOrder);
        }
    }
}
