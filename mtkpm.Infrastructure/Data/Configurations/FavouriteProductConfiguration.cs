using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Business;

namespace mtkpm.Infrastructure.Data.Configurations
{
    public class FavouriteProductConfiguration : IEntityTypeConfiguration<FavouriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavouriteProduct> builder)
        {
            builder.ToTable("FavouriteProducts");

            builder.HasKey(fp => fp.Id);

            builder.Property(fp => fp.AddedAt)
                .IsRequired();

            builder.HasOne(fp => fp.User)
                .WithMany(u => u.FavouriteProducts)
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fp => fp.Product)
                .WithMany()
                .HasForeignKey(fp => fp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(fp => new { fp.UserId, fp.ProductId })
                .IsUnique();
            
            builder.HasIndex(fp => fp.IsDeleted);
        }
    }
}
