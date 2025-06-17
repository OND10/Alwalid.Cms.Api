using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Product
{
    public interface IProductRepository
    {
        // CRUD Operations
        Task<Entities.Product?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Product>> GetAllAsync();
        Task<Entities.Product> CreateAsync(Entities.Product product);
        Task<Entities.Product> UpdateAsync(Entities.Product product);
        Task<bool> DeleteAsync(int id);
        
        // Product-specific operations
        Task<IEnumerable<Entities.Product>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<Entities.Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Entities.Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Entities.Product>> GetLowStockProductsAsync(int threshold = 10);
        Task<IEnumerable<Entities.Product>> GetOutOfStockProductsAsync();
        Task<IEnumerable<Entities.Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Entities.Product>> GetActiveProductsAsync();
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByDepartmentAsync(int departmentId);
        Task<int> GetCountByCategoryAsync(int categoryId);
        Task<bool> ExistsAsync(int id);
        Task<bool> SoftDeleteAsync(int id, string deletedBy);
        Task<bool> UpdateStockAsync(int id, int newStock);
        Task<IEnumerable<Entities.Product>> GetProductsWithImagesAsync();
        Task<IEnumerable<Entities.Product>> GetProductsWithStatisticsAsync();
    }
} 