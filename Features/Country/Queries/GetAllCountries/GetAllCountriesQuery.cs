using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Queries.GetAllCountries
{
    public class GetAllCountriesQuery : IQuery<IEnumerable<CountryResponseDto>>
    {
        // No parameters needed for getting all countries
    }
} 