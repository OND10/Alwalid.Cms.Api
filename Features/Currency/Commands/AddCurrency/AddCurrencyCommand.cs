using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency
{
    public class AddCurrencyCommand : ICommand<CurrencyResponseDto>
    {
        public CurrencyRequestDto Request { get; set; } = new();
    }
} 