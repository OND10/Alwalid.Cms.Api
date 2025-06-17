using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Country
{
    public interface ICountryRepository
    {
        // CRUD Operations
        Task<Entities.Country?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Country>> GetAllAsync();
        Task<Entities.Country> CreateAsync(Entities.Country country);
        Task<Entities.Country> UpdateAsync(Entities.Country country);
        Task<bool> DeleteAsync(int id);
        
        // Country-specific operations
        Task<Entities.Country?> GetByCodeAsync(string code);
        Task<Entities.Country?> GetByNameAsync(string name);
        Task<IEnumerable<Entities.Country>> GetActiveCountriesAsync();
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<int> GetTotalCountAsync();
        Task<bool> ExistsAsync(int id);
    }
} 