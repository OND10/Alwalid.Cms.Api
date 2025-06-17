using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Settings
{
    public interface ISettingsRepository
    {
        // CRUD Operations
        Task<Entities.Settings?> GetByIdAsync(int id);
        Task<IEnumerable<Entities.Settings>> GetAllAsync();
        Task<Entities.Settings> CreateAsync(Entities.Settings settings);
        Task<Entities.Settings> UpdateAsync(Entities.Settings settings);
        Task<bool> DeleteAsync(int id);
        
        // Settings-specific operations
        Task<Entities.Settings?> GetMainSettingsAsync();
        Task<Entities.Settings?> GetBySiteTitleAsync(string siteTitle);
        Task<bool> UpdateMaintenanceModeAsync(bool isMaintenanceMode);
        Task<bool> UpdateDefaultLanguageAsync(string defaultLanguage);
        Task<bool> UpdateDefaultCurrencyAsync(string defaultCurrencyCode);
        Task<int> GetTotalCountAsync();
        Task<bool> ExistsAsync(int id);
        Task<bool> IsMaintenanceModeAsync();
        Task<string?> GetDefaultLanguageAsync();
        Task<string?> GetDefaultCurrencyCodeAsync();
    }
} 