using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Branch
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Branch?> GetByIdAsync(int id)
        {
            return await _context.Branches
                .Include(b => b.Country)
                .Include(b => b.Departments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Entities.Branch>> GetAllAsync()
        {
            return await _context.Branches
                .Include(b => b.Country)
                .Include(b => b.Departments)
                .ToListAsync();
        }

        public async Task<Entities.Branch> CreateAsync(Entities.Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<Entities.Branch> UpdateAsync(Entities.Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
                return false;

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Branch>> GetByCountryIdAsync(int countryId)
        {
            return await _context.Branches
                .Include(b => b.Country)
                .Include(b => b.Departments)
                .Where(b => b.CountryId == countryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Branch>> GetByCityAsync(string city)
        {
            return await _context.Branches
                .Include(b => b.Country)
                .Include(b => b.Departments)
                .Where(b => b.City.Contains(city))
                .ToListAsync();
        }

        public async Task<Entities.Branch?> GetByCityAndAddressAsync(string city, string address)
        {
            return await _context.Branches
                .Include(b => b.Country)
                .Include(b => b.Departments)
                .FirstOrDefaultAsync(b => b.City == city && b.Address == address);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Branches.CountAsync();
        }

        public async Task<int> GetCountByCountryAsync(int countryId)
        {
            return await _context.Branches.CountAsync(b => b.CountryId == countryId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Branches.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsInCountryAsync(int countryId, string city, string address, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.Branches.AnyAsync(b => 
                    b.CountryId == countryId && 
                    b.City == city && 
                    b.Address == address && 
                    b.Id != excludeId.Value);
            
            return await _context.Branches.AnyAsync(b => 
                b.CountryId == countryId && 
                b.City == city && 
                b.Address == address);
        }
    }
} 