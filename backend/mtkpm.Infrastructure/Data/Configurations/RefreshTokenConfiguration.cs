using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedByIp)
                .HasMaxLength(50);

            builder.Property(rt => rt.RevokedReason)
                .HasMaxLength(200);

            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(500);

            builder.Property(rt => rt.DeviceInfo)
                .HasMaxLength(200);

            builder.Property(rt => rt.IpAddress)
                .HasMaxLength(50);

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rt => rt.Token)
                .IsUnique();
            builder.HasIndex(rt => rt.UserId);
            builder.HasIndex(rt => rt.ExpiresAt);

            builder.Ignore(rt => rt.IsExpired);
            builder.Ignore(rt => rt.IsRevoked);
            builder.Ignore(rt => rt.IsActive);
        }
    }
}
