using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Country;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Queries.GetCountryById
{
    public class GetCountryByIdQueryHandler : IQueryHandler<GetCountryByIdQuery, CountryResponseDto?>
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryByIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<CountryResponseDto?>> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var country = await _countryRepository.GetByIdAsync(query.Id);
                
                if (country == null)
                {
                    return await Result<CountryResponseDto?>.FaildAsync(false, "Country not found.");
                }

                var responseDto = new CountryResponseDto
                {
                    Id = country.Id,
                    Name = country.Name,
                    Code = country.Code,
                    BranchesCount = country.Branches?.Count ?? 0
                };

                return await Result<CountryResponseDto?>.SuccessAsync(responseDto, "Country retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CountryResponseDto?>.FaildAsync(false, $"Error retrieving country: {ex.Message}");
            }
        }
    }
} 