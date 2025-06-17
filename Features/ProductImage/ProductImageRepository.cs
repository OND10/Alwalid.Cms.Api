using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.ProductImage
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.ProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task<IEnumerable<Entities.ProductImage>> GetAllAsync()
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .ToListAsync();
        }

        public async Task<Entities.ProductImage> CreateAsync(Entities.ProductImage productImage)
        {
            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }

        public async Task<Entities.ProductImage> UpdateAsync(Entities.ProductImage productImage)
        {
            _context.ProductImages.Update(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
                return false;

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.ProductImage>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Entities.ProductImage?> GetMainImageByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .Where(pi => pi.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteByProductIdAsync(int productId)
        {
            var productImages = await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();

            if (!productImages.Any())
                return false;

            _context.ProductImages.RemoveRange(productImages);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.ProductImages.CountAsync();
        }

        public async Task<int> GetCountByProductAsync(int productId)
        {
            return await _context.ProductImages.CountAsync(pi => pi.ProductId == productId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductImages.AnyAsync(pi => pi.Id == id);
        }

        public async Task<bool> ExistsForProductAsync(int productId, string imageUrl, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.ProductImages.AnyAsync(pi => 
                    pi.ProductId == productId && 
                    pi.ImageUrl == imageUrl && 
                    pi.Id != excludeId.Value);
            
            return await _context.ProductImages.AnyAsync(pi => 
                pi.ProductId == productId && 
                pi.ImageUrl == imageUrl);
        }
    }
} 