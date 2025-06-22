using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsCommandHandler : ICommandHandler<UpdateSettingsCommand, SettingsResponseDto>
    {
        private readonly ISettingsRepository _settingsRepository;

        public UpdateSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<SettingsResponseDto>> Handle(UpdateSettingsCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if settings exists
                var existingSettings = await _settingsRepository.GetByIdAsync(command.Id);
                if (existingSettings == null)
                {
                    return await Result<SettingsResponseDto>.FaildAsync(false, "Settings not found.");
                }

                // Update settings
                existingSettings.DefaultLanguage = command.Request.DefaultLanguage;
                //existingSettings.DefaultCurrencyId = command.Request.DefaultCurrencyId;
                //existingSettings.MaintenanceMode = command.Request.MaintenanceMode;

                var updatedSettings = await _settingsRepository.UpdateAsync(existingSettings);

                var responseDto = new SettingsResponseDto
                {
                    Id = updatedSettings.Id,
                    DefaultLanguage = updatedSettings.DefaultLanguage,
                    //DefaultCurrencyId = updatedSettings.DefaultCurrencyId,
                    //MaintenanceMode = updatedSettings.MaintenanceMode,
                    //CreatedAt = updatedSettings.CreatedAt,
                    //LastModifiedAt = updatedSettings.LastModifiedAt,
                    //IsDeleted = updatedSettings.IsDeleted
                };

                return await Result<SettingsResponseDto>.SuccessAsync(responseDto, "Settings updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<SettingsResponseDto>.FaildAsync(false, $"Error updating settings: {ex.Message}");
            }
        }
    }
} 