using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency
{
    public class DeleteCurrencyCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 