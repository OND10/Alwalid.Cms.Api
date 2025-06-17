using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.ProductStatistic
{
    public interface IProductStatisticRepository
    {
        // CRUD Operations
        Task<Entities.ProductStatistic?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.ProductStatistic>> GetAllAsync();
        Task<Entities.ProductStatistic> CreateAsync(Entities.ProductStatistic productStatistic);
        Task<Entities.ProductStatistic> UpdateAsync(Entities.ProductStatistic productStatistic);
        Task<bool> DeleteAsync(int id);
        
        // ProductStatistic-specific operations
        Task<IEnumerable<Entities.ProductStatistic>> GetByProductIdAsync(int productId);
        Task<IEnumerable<Entities.ProductStatistic>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Entities.ProductStatistic>> GetByProductAndDateRangeAsync(int productId, DateTime startDate, DateTime endDate);
        Task<int> GetTotalQuantitySoldByProductAsync(int productId);
        Task<int> GetTotalQuantitySoldByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Entities.ProductStatistic?> GetTopSellingProductAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Entities.ProductStatistic>> GetTopSellingProductsAsync(int count, DateTime startDate, DateTime endDate);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByProductAsync(int productId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsForProductAndDateAsync(int productId, DateTime date, int? excludeId = null);
    }
} 