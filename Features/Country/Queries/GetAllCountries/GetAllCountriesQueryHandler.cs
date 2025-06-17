using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Country;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IQueryHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>>
    {
        private readonly ICountryRepository _countryRepository;

        public GetAllCountriesQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<IEnumerable<CountryResponseDto>>> Handle(GetAllCountriesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var countries = await _countryRepository.GetAllAsync();
                
                var responseDtos = countries.Select(country => new CountryResponseDto
                {
                    Id = country.Id,
                    Name = country.Name,
                    Code = country.Code,
                    BranchesCount = country.Branches?.Count ?? 0
                });

                return await Result<IEnumerable<CountryResponseDto>>.SuccessAsync(responseDtos, "Countries retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<CountryResponseDto>>.FaildAsync(false, $"Error retrieving countries: {ex.Message}");
            }
        }
    }
} 