using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.ProductImage
{
    public interface IProductImageRepository
    {
        // CRUD Operations
        Task<Entities.ProductImage?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.ProductImage>> GetAllAsync();
        Task<Entities.ProductImage> CreateAsync(Entities.ProductImage productImage);
        Task<Entities.ProductImage> UpdateAsync(Entities.ProductImage productImage);
        Task<bool> DeleteAsync(int id);
        
        // ProductImage-specific operations
        Task<IEnumerable<Entities.ProductImage>> GetByProductIdAsync(int productId);
        Task<Entities.ProductImage?> GetMainImageByProductIdAsync(int productId);
        Task<bool> DeleteByProductIdAsync(int productId);
        Task<int> GetTotalCountAsync();
        Task<int> GetCountByProductAsync(int productId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsForProductAsync(int productId, string imageUrl, int? excludeId = null);
    }
} 