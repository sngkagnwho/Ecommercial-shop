using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mtkpm.Domain.Entities.Business;
using mtkpm.Domain.Entities.Identity_Auth;
using System.Reflection;

namespace mtkpm.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<FavouriteProduct> FavouriteProducts => Set<FavouriteProduct>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.HasQueryFilter<User>(u => !u.IsDeleted);
            modelBuilder.HasQueryFilter<Product>(p => !p.IsDeleted);
            modelBuilder.HasQueryFilter<FavouriteProduct>(f => !f.IsDeleted);

            SeedRoles(modelBuilder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void HasQueryFilter<T>(this ModelBuilder modelBuilder, System.Linq.Expressions.Expression<Func<T, bool>> filter) where T : class
        {
            modelBuilder.Entity<T>().HasQueryFilter(filter);
        }
    }
}
