using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mtkpm.Domain.Entities.Business;
using mtkpm.Domain.Entities.Identity_Auth;
using mtkpm.Infrastructure.Data.Contexts;

namespace mtkpm.Infrastructure.Services.SeedData
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public DataSeeder(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedCategoriesAsync();
            await SeedProductsAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var adminEmail = "admin@mtkpm.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task SeedCategoriesAsync()
        {
            if (await _context.Categories.AnyAsync())
            {
                return;
            }

            var categories = new[]
            {
                new Category("Điện thoại", "Điện thoại thông minh các loại"),
                new Category("Laptop", "Máy tính xách tay"),
                new Category("Tablet", "Máy tính bảng"),
                new Category("Phụ kiện", "Phụ kiện điện tử"),
                new Category("Âm thanh", "Tai nghe, loa, âm thanh")
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        private async Task SeedProductsAsync()
        {
            if (await _context.Products.AnyAsync())
            {
                return;
            }

            var phoneCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Điện thoại");
            var laptopCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Laptop");

            if (phoneCategory != null)
            {
                var products = new[]
                {
                    new Product("iPhone 15 Pro Max", "Apple iPhone 15 Pro Max 256GB", 29990000m, 50, phoneCategory.Id, "https://example.com/iphone15.jpg"),
                    new Product("Samsung Galaxy S24 Ultra", "Samsung Galaxy S24 Ultra 256GB", 27990000m, 30, phoneCategory.Id, "https://example.com/s24.jpg"),
                    new Product("Xiaomi 14 Pro", "Xiaomi 14 Pro 12GB/256GB", 19990000m, 40, phoneCategory.Id, "https://example.com/xiaomi14.jpg")
                };

                await _context.Products.AddRangeAsync(products);
            }

            if (laptopCategory != null)
            {
                var laptops = new[]
                {
                    new Product("MacBook Pro 14", "MacBook Pro 14 inch M3 Pro", 45990000m, 20, laptopCategory.Id, "https://example.com/macbook.jpg"),
                    new Product("Dell XPS 15", "Dell XPS 15 9530 i7-13700H", 35990000m, 15, laptopCategory.Id, "https://example.com/dellxps.jpg")
                };

                await _context.Products.AddRangeAsync(laptops);
            }

            await _context.SaveChangesAsync();
        }
    }
}
