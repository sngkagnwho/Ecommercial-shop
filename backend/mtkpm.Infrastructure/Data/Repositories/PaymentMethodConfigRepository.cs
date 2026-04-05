using mtkpm.Domain.Entities.Business;
using mtkpm.Application.Common.Interfaces.Repositories;
using mtkpm.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace mtkpm.Infrastructure.Data.Repositories
{
    public class PaymentMethodConfigRepository : IPaymentMethodConfigRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodConfigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// L?y t?t c? ph??ng th?c thanh toán
        /// </summary>
        public async Task<List<PaymentMethodConfig>> GetAllAsync()
        {
            return await _context.PaymentMethodConfigs
                .OrderBy(pmc => pmc.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// L?y các ph??ng th?c thanh toán ?ang ho?t ??ng
        /// </summary>
        public async Task<List<PaymentMethodConfig>> GetActiveMethodsAsync()
        {
            return await _context.PaymentMethodConfigs
                .Where(pmc => pmc.IsActive)
                .OrderBy(pmc => pmc.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// L?y ph??ng th?c thanh toán theo ID
        /// </summary>
        public async Task<PaymentMethodConfig?> GetByIdAsync(int id)
        {
            return await _context.PaymentMethodConfigs.FindAsync(id);
        }

        /// <summary>
        /// L?y ph??ng th?c thanh toán theo code
        /// </summary>
        public async Task<PaymentMethodConfig?> GetByCodeAsync(string code)
        {
            return await _context.PaymentMethodConfigs
                .FirstOrDefaultAsync(pmc => pmc.Code.ToLower() == code.ToLower());
        }

        /// <summary>
        /// Tìm ki?m ph??ng th?c thanh toán
        /// </summary>
        public async Task<List<PaymentMethodConfig>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            return await _context.PaymentMethodConfigs
                .Where(pmc => pmc.Code.ToLower().Contains(searchLower) ||
                           pmc.Name.ToLower().Contains(searchLower) ||
                           (pmc.Description != null && pmc.Description.ToLower().Contains(searchLower)))
                .OrderBy(pmc => pmc.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Thêm ph??ng th?c thanh toán m?i
        /// </summary>
        public async Task<PaymentMethodConfig> AddAsync(PaymentMethodConfig paymentMethod)
        {
            _context.PaymentMethodConfigs.Add(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        /// <summary>
        /// C?p nh?t ph??ng th?c thanh toán
        /// </summary>
        public async Task<PaymentMethodConfig> UpdateAsync(PaymentMethodConfig paymentMethod)
        {
            _context.PaymentMethodConfigs.Update(paymentMethod);
            await _context.SaveChangesAsync();
            return paymentMethod;
        }

        /// <summary>
        /// Xóa m?m ph??ng th?c thanh toán
        /// </summary>
        public async Task DeleteAsync(int id, int deletedByUserId)
        {
            var paymentMethod = await GetByIdAsync(id);
            if (paymentMethod != null)
            {
                paymentMethod.SetDeleted(deletedByUserId);
                await UpdateAsync(paymentMethod);
            }
        }

        /// <summary>
        /// Ki?m tra code ph??ng th?c có t?n t?i không
        /// </summary>
        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.PaymentMethodConfigs
                .Where(pmc => pmc.Code.ToLower() == code.ToLower());

            if (excludeId.HasValue)
                query = query.Where(pmc => pmc.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        /// <summary>
        /// Tính t?ng phí (%)
        /// </summary>
        public async Task<decimal> GetAverageFeePercentageAsync()
        {
            var active = await GetActiveMethodsAsync();
            if (active.Count == 0) return 0m;
            return active.Average(pmc => pmc.TransactionFeePercentage);
        }
    }
}
