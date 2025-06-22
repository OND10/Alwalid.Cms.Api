using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;
using System;

namespace Alwalid.Cms.Api.Features.Settings.Queries.GetMainSettings
{
    public class GetMainSettingsQueryHandler : IQueryHandler<GetMainSettingsQuery, SettingsResponseDto>
    {
        private readonly ISettingsRepository _settingsRepository;

        public GetMainSettingsQueryHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<SettingsResponseDto>> Handle(GetMainSettingsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _settingsRepository.GetMainSettingsAsync();

                if (settings == null)
                {
                    return await Result<SettingsResponseDto>.FaildAsync(false, "Main settings not found.");
                }

                var responseDto = new SettingsResponseDto
                {
                    Id = settings.Id,
                    DefaultLanguage = settings.DefaultLanguage,
                    DefaultCurrencyCode = settings.DefaultCurrencyCode,
                    IsMaintenanceMode = settings.IsMaintenanceMode,
                    Address = settings.Address,
                    Copyright = settings.Copyright,
                    Facebook = settings.Facebook,
                    FaviconUrl = settings.FaviconUrl,
                    Instagram = settings.Instagram,
                    LinkedIn = settings.LinkedIn,
                    LogoUrl = settings.LogoUrl,
                    SiteSubtitle = settings.SiteSubtitle,
                    SiteTitle = settings.SiteTitle,
                    Phone = settings.Phone,
                    Twitter = settings.Twitter,
                    Youtube = settings.Youtube,
                    SupportEmail = settings.SupportEmail,
                    Tiktok = settings.Tiktok,
                };

                return await Result<SettingsResponseDto>.SuccessAsync(responseDto, "Main settings retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto>.FaildAsync(false, $"Error retrieving main settings: {ex.Message}");
            }
        }
    }
}