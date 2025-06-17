using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Queries.GetCountryById
{
    public class GetCountryByIdQuery : IQuery<CountryResponseDto?>
    {
        public int Id { get; set; }
    }
} 