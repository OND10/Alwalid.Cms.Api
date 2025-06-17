using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Category
{
    public interface ICategoryRepository
    {
        // CRUD Operations
        Task<Entities.Category?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Category>> GetAllAsync();
        Task<Entities.Category> CreateAsync(Entities.Category category);
        Task<Entities.Category> UpdateAsync(Entities.Category category);
        Task<bool> DeleteAsync(int id);
        
        // Category-specific operations
        Task<IEnumerable<Entities.Category>> GetByDepartmentIdAsync(int departmentId);
        Task<Entities.Category?> GetByEnglishNameAsync(string englishName);
        Task<Entities.Category?> GetByArabicNameAsync(string arabicName);
        Task<IEnumerable<Entities.Category>> GetActiveCategoriesAsync();
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByDepartmentAsync(int departmentId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsInDepartmentAsync(int departmentId, string englishName, string arabicName, int? excludeId = null);
        Task<bool> SoftDeleteAsync(int id, string deletedBy);
    }
} 