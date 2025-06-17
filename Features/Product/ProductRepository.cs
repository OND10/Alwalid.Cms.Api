using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Shared;

namespace Alwalid.Cms.Api.Features.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Statistics)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Entities.Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Statistics)
                .ToListAsync();
        }

        public async Task<Entities.Product> CreateAsync(Entities.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Entities.Product> UpdateAsync(Entities.Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Product>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetLowStockProductsAsync(int threshold = 10)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Stock <= threshold && p.Stock > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetOutOfStockProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Stock == 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.EnglishName.Contains(searchTerm) || 
                           p.ArabicName.Contains(searchTerm) ||
                           p.EnglishDescription.Contains(searchTerm) ||
                           p.ArabicDescription.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetCountByDepartmentAsync(int departmentId)
        {
            return await _context.Products.CountAsync(p => p.DepartmentId == departmentId);
        }

        public async Task<int> GetCountByCategoryAsync(int categoryId)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == categoryId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> SoftDeleteAsync(int id, string deletedBy)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            product.IsDeleted = true;
            product.DeletedBy = deletedBy;
            product.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStockAsync(int id, int newStock)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            product.Stock = newStock;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Product>> GetProductsWithImagesAsync()
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Images.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Product>> GetProductsWithStatisticsAsync()
        {
            return await _context.Products
                .Include(p => p.Department)
                .Include(p => p.Category)
                .Include(p => p.Statistics)
                .Where(p => p.Statistics.Any())
                .ToListAsync();
        }
    }
} 