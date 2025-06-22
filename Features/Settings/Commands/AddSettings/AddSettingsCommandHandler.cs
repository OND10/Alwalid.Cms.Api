using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.AddSettings
{
    public class AddSettingsCommandHandler : ICommandHandler<AddSettingsCommand, SettingsResponseDto>
    {
        private readonly ISettingsRepository _settingsRepository;

        public AddSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<SettingsResponseDto>> Handle(AddSettingsCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var settings = new Entities.Settings
                {
                    DefaultLanguage = command.Request.DefaultLanguage,
                    DefaultCurrencyCode = command.Request.DefaultCurrencyCode,
                    IsMaintenanceMode = command.Request.IsMaintenanceMode,
                    Address = command.Request.Address,
                    Copyright = command.Request.Copyright,
                    Facebook = command.Request.Facebook,
                    FaviconUrl = command.Request.FaviconUrl,
                    Instagram = command.Request.Instagram,
                    LinkedIn = command.Request.LinkedIn,
                    LogoUrl = command.Request.LogoUrl,
                    SiteSubtitle = command.Request.SiteSubtitle,
                    SiteTitle = command.Request.SiteTitle,
                    Phone = command.Request.Phone,
                    Twitter = command.Request.Twitter,
                    Youtube = command.Request.Youtube,
                    SupportEmail = command.Request.SupportEmail,
                    Tiktok = command.Request.Tiktok,
                };

                var createdSettings = await _settingsRepository.CreateAsync(settings);

                var responseDto = new SettingsResponseDto
                {
                    Id = createdSettings.Id,
                    DefaultLanguage = createdSettings.DefaultLanguage,
                    DefaultCurrencyCode = createdSettings.DefaultCurrencyCode,
                    IsMaintenanceMode = createdSettings.IsMaintenanceMode,
                    Address = createdSettings.Address,
                    Copyright = createdSettings.Copyright,
                    Facebook = createdSettings.Facebook,
                    FaviconUrl = createdSettings.FaviconUrl,
                    Instagram = createdSettings.Instagram,
                    LinkedIn = createdSettings.LinkedIn,
                    LogoUrl = createdSettings.LogoUrl,
                    SiteSubtitle = createdSettings.SiteSubtitle,
                    SiteTitle = createdSettings.SiteTitle,
                    Phone = createdSettings.Phone,
                    Twitter = createdSettings.Twitter,
                    Youtube = createdSettings.Youtube,
                    SupportEmail = createdSettings.SupportEmail,
                    Tiktok = createdSettings.Tiktok,
                };

                return await Result<SettingsResponseDto>.SuccessAsync(responseDto, "Settings created successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto>.FaildAsync(false, $"Error creating settings: {ex.Message}");
            }
        }
    }
}