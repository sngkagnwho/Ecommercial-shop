using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mtkpm.Domain.Entities.Identity_Auth;

namespace mtkpm.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasIndex(u => u.IsDeleted);
            builder.HasIndex(u => u.Email);
            builder.HasIndex(u => u.UserName);
        }
    }
}
