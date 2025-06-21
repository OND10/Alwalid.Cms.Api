using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Department
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Entities.Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .ToListAsync();
        }

        public async Task<Entities.Department> CreateAsync(Entities.Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Entities.Department> UpdateAsync(Entities.Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Department>> GetByBranchIdAsync(int branchId)
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .Where(d => d.BranchId == branchId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Department>> GetByBranchAndNameAsync(int branchId, string name)
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .Where(d => d.BranchId == branchId && 
                           (d.EnglishName.Contains(name) || d.ArabicName.Contains(name)))
                .ToListAsync();
        }

        public async Task<Entities.Department?> GetByEnglishNameAsync(string englishName)
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.EnglishName == englishName);
        }

        public async Task<Entities.Department?> GetByArabicNameAsync(string arabicName)
        {
            return await _context.Departments
                .Include(d => d.Branch)
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.ArabicName == arabicName);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Departments.CountAsync();
        }

        public async Task<int> GetCountByBranchAsync(int branchId)
        {
            return await _context.Departments.CountAsync(d => d.BranchId == branchId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Departments.AnyAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsInBranchAsync(int branchId, string englishName, string arabicName, int? excludeId = null)
        {  
            return await _context.Departments.AnyAsync(d => 
                d.BranchId == branchId && 
                (d.EnglishName == englishName || d.ArabicName == arabicName));
        }
    }
} 