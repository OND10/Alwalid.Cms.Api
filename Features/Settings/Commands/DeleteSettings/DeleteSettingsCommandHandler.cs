using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Settings;

namespace Alwalid.Cms.Api.Features.Settings.Commands.DeleteSettings
{
    public class DeleteSettingsCommandHandler : ICommandHandler<DeleteSettingsCommand, bool>
    {
        private readonly ISettingsRepository _settingsRepository;

        public DeleteSettingsCommandHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Result<bool>> Handle(DeleteSettingsCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if settings exists
                if (!await _settingsRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Settings not found.");
                }

                // Delete settings
                var isDeleted = await _settingsRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Settings deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete settings.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting settings: {ex.Message}");
            }
        }
    }
}