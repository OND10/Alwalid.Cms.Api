using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency
{
    public class UpdateDefaultCurrencyCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string DefaultCurrency { get; set; }
    }
}