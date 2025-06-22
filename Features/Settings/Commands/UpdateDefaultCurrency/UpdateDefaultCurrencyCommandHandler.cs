using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Settings;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency
{
    public class UpdateDefaultCurrencyCommandHandler : ICommandHandler<UpdateDefaultCurrencyCommand, bool>
    {
        private readonly ISettingsRepository _settingsRepository;

        public UpdateDefaultCurrencyCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<bool>> Handle(UpdateDefaultCurrencyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if settings exists
                if (!await _settingsRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Settings not found.");
                }

                // Update default currency
                var isUpdated = await _settingsRepository.UpdateDefaultCurrencyAsync(command.Request.DefaultCurrency);

                if (isUpdated)
                {
                    return await Result<bool>.SuccessAsync(true, "Default currency updated successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to update default currency.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error updating default currency: {ex.Message}");
            }
        }
    }
} 