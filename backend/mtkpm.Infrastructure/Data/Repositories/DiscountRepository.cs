using mtkpm.Domain.Entities.Business;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _context;

        public DiscountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// L?y chi?t kh?u theo mã
        /// </summary>
        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(d => d.Code.ToLower() == code.ToLower() && d.IsActive);
        }

        /// <summary>
        /// L?y t?t c? chi?t kh?u còn hi?u l?c
        /// </summary>
        public async Task<List<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Discounts
                .Where(d => d.IsActive && d.StartDate <= now && d.EndDate > now)
                .OrderByDescending(d => d.CreateAt)
                .ToListAsync();
        }

        /// <summary>
        /// L?y t?t c? chi?t kh?u (bao g?m ?ã h?t h?n) - Admin only
        /// </summary>
        public async Task<List<Discount>> GetAllDiscountsAsync()
        {
            return await _context.Discounts
                .OrderByDescending(d => d.CreateAt)
                .ToListAsync();
        }

        /// <summary>
        /// L?y chi?t kh?u theo ID
        /// </summary>
        public async Task<Discount?> GetByIdAsync(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        /// <summary>
        /// Thêm chi?t kh?u m?i
        /// </summary>
        public async Task<Discount> AddAsync(Discount discount)
        {
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        /// <summary>
        /// C?p nh?t chi?t kh?u
        /// </summary>
        public async Task<Discount> UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        /// <summary>
        /// Xóa m?m chi?t kh?u
        /// </summary>
        public async Task DeleteAsync(int id, int deletedByUserId)
        {
            var discount = await GetByIdAsync(id);
            if (discount != null)
            {
                discount.SetDeleted(deletedByUserId);
                await UpdateAsync(discount);
            }
        }

        /// <summary>
        /// Ki?m tra mã chi?t kh?u ?ã t?n t?i không
        /// </summary>
        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.Discounts
                .Where(d => d.Code.ToLower() == code.ToLower());

            if (excludeId.HasValue)
                query = query.Where(d => d.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// L?y l?ch s? s? d?ng chi?t kh?u
        /// </summary>
        public async Task<List<DiscountUsageHistory>> GetUsageHistoryAsync(int discountId)
        {
            return await _context.DiscountUsageHistories
                .Where(h => h.DiscountId == discountId)
                .OrderByDescending(h => h.UsedAt)
                .ToListAsync();
        }

        /// <summary>
        /// L?y s? l?n user ?ã s? d?ng chi?t kh?u
        /// </summary>
        public async Task<int> GetUserUsageCountAsync(int discountId, int userId)
        {
            return await _context.DiscountUsageHistories
                .CountAsync(h => h.DiscountId == discountId && h.UserId == userId);
        }

        /// <summary>
        /// Thêm l?ch s? s? d?ng chi?t kh?u
        /// </summary>
        public async Task AddUsageHistoryAsync(DiscountUsageHistory history)
        {
            _context.DiscountUsageHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Tìm ki?m chi?t kh?u - Admin
        /// </summary>
        public async Task<List<Discount>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            return await _context.Discounts
                .Where(d => d.Code.ToLower().Contains(searchLower) ||
                           d.Name.ToLower().Contains(searchLower) ||
                           (d.Description != null && d.Description.ToLower().Contains(searchLower)))
                .OrderByDescending(d => d.CreateAt)
                .ToListAsync();
        }

        /// <summary>
        /// Ki?m tra xem user có th? s? d?ng chi?t kh?u không
        /// </summary>
        public async Task<(bool canUse, string reason)> CanUserUseDiscountAsync(int discountId, int userId, decimal orderAmount)
        {
            var discount = await GetByIdAsync(discountId);

            if (discount == null)
                return (false, "Chi?t kh?u không t?n t?i");

            if (discount.IsExpired)
                return (false, "Chi?t kh?u ?ã h?t h?n");

            if (discount.IsBudgetExhausted)
                return (false, "Chi?t kh?u ?ã h?t ngân sách");

            if (discount.IsUsageLimitReached)
                return (false, "Chi?t kh?u ?ã ??t s? l?n s? d?ng t?i ?a");

            if (discount.MinimumOrderAmount.HasValue && orderAmount < discount.MinimumOrderAmount.Value)
                return (false, $"Giá tr? ??n hàng ph?i t?i thi?u {discount.MinimumOrderAmount:C}");

            if (discount.MaxUsagePerUser.HasValue)
            {
                var userUsageCount = await GetUserUsageCountAsync(discountId, userId);
                if (userUsageCount >= discount.MaxUsagePerUser.Value)
                    return (false, $"B?n ?ã s? d?ng chi?t kh?u này t?i ?a {discount.MaxUsagePerUser.Value} l?n");
            }

            if (discount.IsNewUserOnly)
            {
                // Ki?m tra xem user ?ã mua hàng ch?a
                var userHasOrders = await _context.Orders
                    .AnyAsync(o => o.UserId == userId);

                if (userHasOrders)
                    return (false, "Chi?t kh?u này ch? dành cho user m?i");
            }

            return (true, "OK");
        }
    }
}
