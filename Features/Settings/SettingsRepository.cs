using Microsoft.EntityFrameworkCore;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Settings?> GetByIdAsync(int id)
        {
            return await _context.Settings
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Entities.Settings>> GetAllAsync()
        {
            return await _context.Settings
                .ToListAsync();
        }

        public async Task<Entities.Settings> CreateAsync(Entities.Settings settings)
        {
            await _context.Settings.AddAsync(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<Entities.Settings> UpdateAsync(Entities.Settings settings)
        {
            settings.LastUpdated = DateTime.UtcNow;
            _context.Settings.Update(settings);
            await _context.SaveChangesAsync();
            return settings;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var settings = await _context.Settings.FindAsync(id);
            if (settings == null)
                return false;

            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Entities.Settings?> GetMainSettingsAsync()
        {
            return await _context.Settings
                .OrderByDescending(s => s.LastUpdated)
                .FirstOrDefaultAsync();
        }

        public async Task<Entities.Settings?> GetBySiteTitleAsync(string siteTitle)
        {
            return await _context.Settings
                .FirstOrDefaultAsync(s => s.SiteTitle == siteTitle);
        }

        public async Task<bool> UpdateMaintenanceModeAsync(bool isMaintenanceMode)
        {
            var settings = await GetMainSettingsAsync();
            if (settings == null)
                return false;

            settings.IsMaintenanceMode = isMaintenanceMode;
            settings.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDefaultLanguageAsync(string defaultLanguage)
        {
            var settings = await GetMainSettingsAsync();
            if (settings == null)
                return false;

            settings.DefaultLanguage = defaultLanguage;
            settings.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDefaultCurrencyAsync(string defaultCurrencyCode)
        {
            var settings = await GetMainSettingsAsync();
            if (settings == null)
                return false;

            settings.DefaultCurrencyCode = defaultCurrencyCode;
            settings.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Settings.CountAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Settings.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> IsMaintenanceModeAsync()
        {
            var settings = await GetMainSettingsAsync();
            return settings?.IsMaintenanceMode ?? false;
        }

        public async Task<string?> GetDefaultLanguageAsync()
        {
            var settings = await GetMainSettingsAsync();
            return settings?.DefaultLanguage;
        }

        public async Task<string?> GetDefaultCurrencyCodeAsync()
        {
            var settings = await GetMainSettingsAsync();
            return settings?.DefaultCurrencyCode;
        }
    }
} 