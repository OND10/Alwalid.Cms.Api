using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.ProductStatistic
{
    public class ProductStatisticRepository : IProductStatisticRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductStatisticRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.ProductStatistic?> GetByIdAsync(int id)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task<IEnumerable<Entities.ProductStatistic>> GetAllAsync()
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .ToListAsync();
        }

        public async Task<Entities.ProductStatistic> CreateAsync(Entities.ProductStatistic productStatistic)
        {
            await _context.ProductStatistics.AddAsync(productStatistic);
            await _context.SaveChangesAsync();
            return productStatistic;
        }

        public async Task<Entities.ProductStatistic> UpdateAsync(Entities.ProductStatistic productStatistic)
        {
            _context.ProductStatistics.Update(productStatistic);
            await _context.SaveChangesAsync();
            return productStatistic;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var productStatistic = await _context.ProductStatistics.FindAsync(id);
            if (productStatistic == null)
                return false;

            _context.ProductStatistics.Remove(productStatistic);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.ProductStatistic>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .Where(ps => ps.ProductId == productId)
                .OrderByDescending(ps => ps.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.ProductStatistic>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .Where(ps => ps.Date >= startDate && ps.Date <= endDate)
                .OrderByDescending(ps => ps.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Entities.ProductStatistic>> GetByProductAndDateRangeAsync(int productId, DateTime startDate, DateTime endDate)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .Where(ps => ps.ProductId == productId && ps.Date >= startDate && ps.Date <= endDate)
                .OrderByDescending(ps => ps.Date)
                .ToListAsync();
        }

        public async Task<int> GetTotalQuantitySoldByProductAsync(int productId)
        {
            return await _context.ProductStatistics
                .Where(ps => ps.ProductId == productId)
                .SumAsync(ps => ps.QuantitySold);
        }

        public async Task<int> GetTotalQuantitySoldByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductStatistics
                .Where(ps => ps.Date >= startDate && ps.Date <= endDate)
                .SumAsync(ps => ps.QuantitySold);
        }

        public async Task<Entities.ProductStatistic?> GetTopSellingProductAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .Where(ps => ps.Date >= startDate && ps.Date <= endDate)
                .OrderByDescending(ps => ps.QuantitySold)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entities.ProductStatistic>> GetTopSellingProductsAsync(int count, DateTime startDate, DateTime endDate)
        {
            return await _context.ProductStatistics
                .Include(ps => ps.Product)
                .Where(ps => ps.Date >= startDate && ps.Date <= endDate)
                .OrderByDescending(ps => ps.QuantitySold)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.ProductStatistics.CountAsync();
        }

        public async Task<int> GetCountByProductAsync(int productId)
        {
            return await _context.ProductStatistics.CountAsync(ps => ps.ProductId == productId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductStatistics.AnyAsync(ps => ps.Id == id);
        }

        public async Task<bool> ExistsForProductAndDateAsync(int productId, DateTime date, int? excludeId = null)
        {
            if (excludeId.HasValue)
                return await _context.ProductStatistics.AnyAsync(ps => 
                    ps.ProductId == productId && 
                    ps.Date.Date == date.Date && 
                    ps.Id != excludeId.Value);
            
            return await _context.ProductStatistics.AnyAsync(ps => 
                ps.ProductId == productId && 
                ps.Date.Date == date.Date);
        }
    }
} 