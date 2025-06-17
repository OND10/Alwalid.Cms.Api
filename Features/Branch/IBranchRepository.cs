using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Branch
{
    public interface IBranchRepository
    {
        // CRUD Operations
        Task<Entities.Branch?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Branch>> GetAllAsync();
        Task<Entities.Branch> CreateAsync(Entities.Branch branch);
        Task<Entities.Branch> UpdateAsync(Entities.Branch branch);
        Task<bool> DeleteAsync(int id);
        
        // Branch-specific operations
        Task<IEnumerable<Entities.Branch>> GetByCountryIdAsync(int countryId);
        Task<IEnumerable<Entities.Branch>> GetByCityAsync(string city);
        Task<Entities.Branch?> GetByCityAndAddressAsync(string city, string address);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByCountryAsync(int countryId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsInCountryAsync(int countryId, string city, string address, int? excludeId = null);
    }
} 