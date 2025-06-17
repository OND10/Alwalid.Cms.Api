using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Currency
{
    public interface ICurrencyRepository
    {
        // CRUD Operations
        Task<Entities.Currency?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Currency>> GetAllAsync();
        Task<Entities.Currency> CreateAsync(Entities.Currency currency);
        Task<Entities.Currency> UpdateAsync(Entities.Currency currency);
        Task<bool> DeleteAsync(int id);
        
        // Currency-specific operations
        Task<Entities.Currency?> GetByCodeAsync(string code);
        Task<Entities.Currency?> GetByNameAsync(string name);
        Task<Entities.Currency?> GetBySymbolAsync(string symbol);
        Task<IEnumerable<Entities.Currency>> GetActiveCurrenciesAsync();
        Task<int> GetTotalCountAsync();
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByCodeAsync(string code);
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSymbolUniqueAsync(string symbol, int? excludeId = null);
    }
} 