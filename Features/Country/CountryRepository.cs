using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Country
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Country?> GetByIdAsync(int id)
        {
            return await _context.Countries
                .Include(c => c.Branches)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Entities.Country>> GetAllAsync()
        {
            return await _context.Countries
                .Include(c => c.Branches)
                .ToListAsync();
        }

        public async Task<Entities.Country> CreateAsync(Entities.Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<Entities.Country> UpdateAsync(Entities.Country country)
        {
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
                return false;

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Entities.Country?> GetByCodeAsync(string code)
        {
            return await _context.Countries
                .Include(c => c.Branches)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Entities.Country?> GetByNameAsync(string name)
        {
            return await _context.Countries
                .Include(c => c.Branches)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Entities.Country>> GetActiveCountriesAsync()
        {
            return await _context.Countries
                .Include(c => c.Branches)
                .ToListAsync();
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.Countries.AnyAsync(c => c.Code == code && c.Id != excludeId.Value);
            
            return !await _context.Countries.AnyAsync(c => c.Code == code);
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.Countries.AnyAsync(c => c.Name == name && c.Id != excludeId.Value);
            
            return !await _context.Countries.AnyAsync(c => c.Name == name);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Countries.CountAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Countries.AnyAsync(c => c.Id == id);
        }
    }
} 