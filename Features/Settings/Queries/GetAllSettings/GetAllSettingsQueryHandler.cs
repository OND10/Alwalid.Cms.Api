using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;
using Microsoft.Extensions.Caching.Memory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetAllSettings
{
    public class GetAllSettingsQueryHandler : IQueryHandler<GetAllSettingsQuery, IEnumerable<SettingsResponseDto>>
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetAllSettings";

        public GetAllSettingsQueryHandler(ISettingsRepository settingsRepository, IMemoryCache memoryCache)
        {
            _settingsRepository = settingsRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<SettingsResponseDto>>> Handle(GetAllSettingsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<SettingsResponseDto>>? settings))
                {
                    var result = await _settingsRepository.GetAllAsync();

                    var responseDtos = result.Select(setting => new SettingsResponseDto
                    {
                        Id = setting.Id,
                        DefaultLanguage = setting.DefaultLanguage,
                        DefaultCurrencyCode = setting.DefaultCurrencyCode,
                        IsMaintenanceMode = setting.IsMaintenanceMode,
                        Address = setting.Address,
                        Copyright = setting.Copyright,
                        Facebook = setting.Facebook,
                        FaviconUrl = setting.FaviconUrl,
                        Instagram = setting.Instagram,
                        LinkedIn = setting.LinkedIn,
                        LogoUrl = setting.LogoUrl,
                        SiteSubtitle = setting.SiteSubtitle,
                        SiteTitle = setting.SiteTitle,
                        Phone = setting.Phone,
                        Twitter = setting.Twitter,
                        Youtube = setting.Youtube,
                        SupportEmail = setting.SupportEmail,
                        Tiktok = setting.Tiktok,
                    });

                    settings = await Result<IEnumerable<SettingsResponseDto>>.SuccessAsync(responseDtos, "Settings retrieved successfully.", true);

                    if (settings.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, settings, cacheExpiryOptions);

                        return await Result<IEnumerable<SettingsResponseDto>>.SuccessAsync(settings.Data, "Settings retrieved successfully.", true);
                    }
                }

                return settings ?? await Result<IEnumerable<SettingsResponseDto>>.FaildAsync(false, "No settings found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<SettingsResponseDto>>.FaildAsync(false, $"Error retrieving settings: {ex.Message}");
            }
        }
    }
}