using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Settings;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage
{
    public class UpdateDefaultLanguageCommandHandler : ICommandHandler<UpdateDefaultLanguageCommand, bool>
    {
        private readonly ISettingsRepository _settingsRepository;

        public UpdateDefaultLanguageCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<bool>> Handle(UpdateDefaultLanguageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if settings exists
                if (!await _settingsRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Settings not found.");
                }

                // Update default language
                var isUpdated = await _settingsRepository.UpdateDefaultLanguageAsync(command.DefaultLanguage);

                if (isUpdated)
                {
                    return await Result<bool>.SuccessAsync(true, "Default language updated successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to update default language.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error updating default language: {ex.Message}");
            }
        }
    }
}