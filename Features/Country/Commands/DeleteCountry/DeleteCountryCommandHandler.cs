using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Country;

namespace Alwalid.Cms.Api.Features.Country.Commands.DeleteCountry
{
    public class DeleteCountryCommandHandler : ICommandHandler<DeleteCountryCommand, bool>
    {
        private readonly ICountryRepository _countryRepository;

        public DeleteCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<bool>> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if country exists
                if (await _countryRepository.ExistsAsync(command.Id) is true)
                {
                    return await Result<bool>.FaildAsync(false, "Country not found.");
                }

                // Delete country
                var isDeleted = await _countryRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Country deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete country.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting country: {ex.Message}");
            }
        }
    }
} 