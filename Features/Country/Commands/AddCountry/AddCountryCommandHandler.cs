using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Country;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Commands.AddCountry
{
    public class AddCountryCommandHandler : ICommandHandler<AddCountryCommand, CountryResponseDto>
    {
        private readonly ICountryRepository _countryRepository;

        public AddCountryCommandHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<CountryResponseDto>> Handle(AddCountryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate unique constraints
                if (!await _countryRepository.IsCodeUniqueAsync(command.Request.Code))
                {
                    return await Result<CountryResponseDto>.FaildAsync(false, "Country code already exists.");
                }

                if (!await _countryRepository.IsNameUniqueAsync(command.Request.Name))
                {
                    return await Result<CountryResponseDto>.FaildAsync(false, "Country name already exists.");
                }

                // Create new country
                var country = new Entities.Country
                {
                    Name = command.Request.Name,
                    Code = command.Request.Code
                };

                var createdCountry = await _countryRepository.CreateAsync(country);

                // Map to response DTO
                var responseDto = new CountryResponseDto
                {
                    Id = createdCountry.Id,
                    Name = createdCountry.Name,
                    Code = createdCountry.Code,
                    BranchesCount = createdCountry.Branches?.Count ?? 0
                };

                return await Result<CountryResponseDto>.SuccessAsync(responseDto, "Country created successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CountryResponseDto>.FaildAsync(false, $"Error creating country: {ex.Message}");
            }
        }
    }
} 