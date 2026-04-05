using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IDiscountRepository
    {
        Task<Discount?> GetByCodeAsync(string code);
        Task<List<Discount>> GetActiveDiscountsAsync();
        Task<List<Discount>> GetAllDiscountsAsync();
        Task<Discount?> GetByIdAsync(int id);
        Task<Discount> AddAsync(Discount discount);
        Task<Discount> UpdateAsync(Discount discount);
        Task DeleteAsync(int id, int deletedByUserId);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<List<DiscountUsageHistory>> GetUsageHistoryAsync(int discountId);
        Task<int> GetUserUsageCountAsync(int discountId, int userId);
        Task AddUsageHistoryAsync(DiscountUsageHistory history);
        Task<List<Discount>> SearchAsync(string searchTerm);
        Task<(bool canUse, string reason)> CanUserUseDiscountAsync(int discountId, int userId, decimal orderAmount);
    }
}
