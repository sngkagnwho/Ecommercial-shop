using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IPricingRuleRepository
    {
        Task<List<PricingRule>> GetAllAsync();
        Task<PricingRule?> GetByIdAsync(int id);
        Task<List<PricingRule>> GetActiveRulesAsync();
        Task<List<PricingRule>> SearchAsync(string searchTerm);
        Task<PricingRule> AddAsync(PricingRule pricingRule);
        Task<PricingRule> UpdateAsync(PricingRule pricingRule);
        Task DeleteAsync(int id, int deletedByUserId);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<List<PricingRule>> GetByRuleTypeAsync(string ruleType);
    }
}
