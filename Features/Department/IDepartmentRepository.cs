using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Department
{
    public interface IDepartmentRepository
    {
        // CRUD Operations
        Task<Entities.Department?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Department>> GetAllAsync();
        Task<Entities.Department> CreateAsync(Entities.Department department);
        Task<Entities.Department> UpdateAsync(Entities.Department department);
        Task<bool> DeleteAsync(int id);
        
        // Department-specific operations
        Task<IEnumerable<Entities.Department>> GetByBranchIdAsync(int branchId);
        Task<IEnumerable<Entities.Department>> GetByBranchAndNameAsync(int branchId, string name);
        Task<Entities.Department?> GetByEnglishNameAsync(string englishName);
        Task<Entities.Department?> GetByArabicNameAsync(string arabicName);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByBranchAsync(int branchId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsInBranchAsync(int branchId, string englishName, string arabicName, int? excludeId = null);
    }
} 