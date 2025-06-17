using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Commands.AddCountry
{
    public class AddCountryCommand : ICommand<CountryResponseDto>
    {
        public CountryRequestDto Request { get; set; } = new();
    }
} 