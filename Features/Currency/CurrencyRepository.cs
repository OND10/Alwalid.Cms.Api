using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Currency
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Currency?> GetByIdAsync(int id)
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Entities.Currency>> GetAllAsync()
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Entities.Currency> CreateAsync(Entities.Currency currency)
        {
            await _context.Currencies.AddAsync(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        public async Task<Entities.Currency> UpdateAsync(Entities.Currency currency)
        {
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
                return false;

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Entities.Currency?> GetByCodeAsync(string code)
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Entities.Currency?> GetByNameAsync(string name)
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Entities.Currency?> GetBySymbolAsync(string symbol)
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Symbol == symbol);
        }

        public async Task<IEnumerable<Entities.Currency>> GetActiveCurrenciesAsync()
        {
            return await _context.Currencies
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Currencies.CountAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Currencies.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.Currencies.AnyAsync(c => c.Code == code && c.Id != excludeId.Value);
            
            return !await _context.Currencies.AnyAsync(c => c.Code == code);
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.Currencies.AnyAsync(c => c.Name == name && c.Id != excludeId.Value);
            
            return !await _context.Currencies.AnyAsync(c => c.Name == name);
        }

        public async Task<bool> IsSymbolUniqueAsync(string symbol, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _context.Currencies.AnyAsync(c => c.Symbol == symbol && c.Id != excludeId.Value);
            
            return !await _context.Currencies.AnyAsync(c => c.Symbol == symbol);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Currencies.AnyAsync(c => c.Code == code);
        }
    }
} 