using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Settings;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode
{
    public class UpdateMaintenanceModeCommandHandler : ICommandHandler<UpdateMaintenanceModeCommand, bool>
    {
        private readonly ISettingsRepository _settingsRepository;

        public UpdateMaintenanceModeCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<bool>> Handle(UpdateMaintenanceModeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if settings exists
                if (!await _settingsRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Settings not found.");
                }

                // Update maintenance mode
                var isUpdated = await _settingsRepository.UpdateMaintenanceModeAsync(command.MaintenanceMode);

                if (isUpdated)
                {
                    return await Result<bool>.SuccessAsync(true, "Maintenance mode updated successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to update maintenance mode.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error updating maintenance mode: {ex.Message}");
            }
        }
    }
} 