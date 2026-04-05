using mtkpm.Domain.Entities.Business;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class PricingRuleRepository : IPricingRuleRepository
    {
        private readonly ApplicationDbContext _context;

        public PricingRuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// L?y t?t c? quy t?c ??nh giá
        /// </summary>
        public async Task<List<PricingRule>> GetAllAsync()
        {
            return await _context.PricingRules
                .OrderByDescending(pr => pr.Priority)
                .ThenByDescending(pr => pr.CreateAt)
                .ToListAsync();
        }

        /// <summary>
        /// L?y quy t?c ??nh giá theo ID
        /// </summary>
        public async Task<PricingRule?> GetByIdAsync(int id)
        {
            return await _context.PricingRules.FindAsync(id);
        }

        /// <summary>
        /// L?y các quy t?c còn hi?u l?c
        /// </summary>
        public async Task<List<PricingRule>> GetActiveRulesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.PricingRules
                .Where(pr => pr.IsActive && pr.StartDate <= now && pr.EndDate > now)
                .OrderByDescending(pr => pr.Priority)
                .ToListAsync();
        }

        /// <summary>
        /// Tìm ki?m quy t?c ??nh giá
        /// </summary>
        public async Task<List<PricingRule>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            return await _context.PricingRules
                .Where(pr => pr.Name.ToLower().Contains(searchLower) ||
                           (pr.Description != null && pr.Description.ToLower().Contains(searchLower)) ||
                           pr.RuleType.ToLower().Contains(searchLower))
                .OrderByDescending(pr => pr.Priority)
                .ToListAsync();
        }

        /// <summary>
        /// Thêm quy t?c ??nh giá m?i
        /// </summary>
        public async Task<PricingRule> AddAsync(PricingRule pricingRule)
        {
            _context.PricingRules.Add(pricingRule);
            await _context.SaveChangesAsync();
            return pricingRule;
        }

        /// <summary>
        /// C?p nh?t quy t?c ??nh giá
        /// </summary>
        public async Task<PricingRule> UpdateAsync(PricingRule pricingRule)
        {
            _context.PricingRules.Update(pricingRule);
            await _context.SaveChangesAsync();
            return pricingRule;
        }

        /// <summary>
        /// Xóa m?m quy t?c
        /// </summary>
        public async Task DeleteAsync(int id, int deletedByUserId)
        {
            var pricingRule = await GetByIdAsync(id);
            if (pricingRule != null)
            {
                pricingRule.SetDeleted(deletedByUserId);
                await UpdateAsync(pricingRule);
            }
        }

        /// <summary>
        /// Ki?m tra tên quy t?c có t?n t?i không
        /// </summary>
        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            var query = _context.PricingRules
                .Where(pr => pr.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
                query = query.Where(pr => pr.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// L?y quy t?c theo lo?i
        /// </summary>
        public async Task<List<PricingRule>> GetByRuleTypeAsync(string ruleType)
        {
            return await _context.PricingRules
                .Where(pr => pr.RuleType == ruleType && pr.IsActive)
                .OrderByDescending(pr => pr.Priority)
                .ToListAsync();
        }
    }
}
