using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Country.Dtos;

namespace Alwalid.Cms.Api.Features.Country.Commands.UpdateCountry
{
    public class UpdateCountryCommand : ICommand<CountryResponseDto>
    {
        public int Id { get; set; }
        public CountryRequestDto Request { get; set; } = new();
    }
} 