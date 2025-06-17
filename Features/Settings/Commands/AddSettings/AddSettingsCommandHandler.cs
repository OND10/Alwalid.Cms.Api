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
                    //DefaultCurrencyId = command.Request.DefaultCurrencyId,
                    //MaintenanceMode = command.Request.MaintenanceMode
                };

                var createdSettings = await _settingsRepository.CreateAsync(settings);

                var responseDto = new SettingsResponseDto
                {
                    Id = createdSettings.Id,
                    DefaultLanguage = createdSettings.DefaultLanguage,
                    //DefaultCurrencyId = createdSettings.DefaultCurrencyId,
                    //MaintenanceMode = createdSettings.MaintenanceMode,
                    //CreatedAt = createdSettings.CreatedAt,
                    //LastModifiedAt = createdSettings.LastModifiedAt,
                    //IsDeleted = createdSettings.IsDeleted
                };

                return await Result<SettingsResponseDto>.SuccessAsync(responseDto, "Settings created successfully.");
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto>.FaildAsync(false, $"Error creating settings: {ex.Message}");
            }
        }
    }
} 