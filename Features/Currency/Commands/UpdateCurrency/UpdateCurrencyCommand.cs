using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency
{
    public class UpdateCurrencyCommand : ICommand<CurrencyResponseDto>
    {
        public int Id { get; set; }
        public CurrencyRequestDto Request { get; set; } = new();
    }
} 