using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Country.Commands.DeleteCountry
{
    public class DeleteCountryCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 