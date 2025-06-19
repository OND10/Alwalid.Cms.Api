using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Country;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Commands.UpdateCountry
{
    public class UpdateCountryCommandHandler : ICommandHandler<UpdateCountryCommand, CountryResponseDto>
    {
        private readonly ICountryRepository _countryRepository;

        public UpdateCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<CountryResponseDto>> Handle(UpdateCountryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if country exists
                var existingCountry = await _countryRepository.GetByIdAsync(command.Id);
                if (existingCountry == null)
                {
                    return await Result<CountryResponseDto>.FaildAsync(false, "Country not found.");
                }

                // Validate unique constraints
                if (await _countryRepository.IsCodeUniqueAsync(command.Request.Code) is true)
                {
                    return await Result<CountryResponseDto>.FaildAsync(false, "Country code already exists.");
                }

                if (await _countryRepository.IsNameUniqueAsync(command.Request.Name) is true)
                {
                    return await Result<CountryResponseDto>.FaildAsync(false, "Country name already exists.");
                }

                // Update country
                existingCountry.Name = command.Request.Name;
                existingCountry.Code = command.Request.Code;

                var updatedCountry = await _countryRepository.UpdateAsync(existingCountry);

                // Map to response DTO
                var responseDto = new CountryResponseDto
                {
                    Id = updatedCountry.Id,
                    Name = updatedCountry.Name,
                    Code = updatedCountry.Code,
                    BranchesCount = updatedCountry.Branches?.Count ?? 0
                };

                return await Result<CountryResponseDto>.SuccessAsync(responseDto, "Country updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<CountryResponseDto>.FaildAsync(false, $"Error updating country: {ex.Message}");
            }
        }
    }
} 