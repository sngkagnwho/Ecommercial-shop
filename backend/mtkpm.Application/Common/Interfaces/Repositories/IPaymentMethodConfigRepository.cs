using mtkpm.Domain.Entities.Business;

namespace mtkpm.Application.Common.Interfaces.Repositories
{
    public interface IPaymentMethodConfigRepository
    {
        Task<List<PaymentMethodConfig>> GetAllAsync();
        Task<List<PaymentMethodConfig>> GetActiveMethodsAsync();
        Task<PaymentMethodConfig?> GetByIdAsync(int id);
        Task<PaymentMethodConfig?> GetByCodeAsync(string code);
        Task<List<PaymentMethodConfig>> SearchAsync(string searchTerm);
        Task<PaymentMethodConfig> AddAsync(PaymentMethodConfig paymentMethod);
        Task<PaymentMethodConfig> UpdateAsync(PaymentMethodConfig paymentMethod);
        Task DeleteAsync(int id, int deletedByUserId);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<decimal> GetAverageFeePercentageAsync();
    }
}
