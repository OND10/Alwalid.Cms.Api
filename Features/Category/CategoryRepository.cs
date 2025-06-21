using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Shared;

namespace Alwalid.Cms.Api.Features.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Entities.Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Entities.Category> CreateAsync(Entities.Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Entities.Category> UpdateAsync(Entities.Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Category>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .Where(c => c.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<Entities.Category?> GetByEnglishNameAsync(string englishName)
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.EnglishName == englishName);
        }

        public async Task<Entities.Category?> GetByArabicNameAsync(string arabicName)
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.ArabicName == arabicName);
        }

        public async Task<IEnumerable<Entities.Category>> GetActiveCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Department)
                .Include(c => c.Products)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<int> GetCountByDepartmentAsync(int departmentId)
        {
            return await _context.Categories.CountAsync(c => c.DepartmentId == departmentId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsInDepartmentAsync(int departmentId, string englishName, string arabicName, int? excludeId = null)
        {
            return await _context.Categories.AnyAsync(c => 
                c.DepartmentId == departmentId && 
                (c.EnglishName == englishName || c.ArabicName == arabicName));
        }

        public async Task<bool> SoftDeleteAsync(int id, string deletedBy)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            category.IsDeleted = true;
            category.DeletedBy = deletedBy;
            category.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 