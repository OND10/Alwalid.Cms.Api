using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetBySymbol
{
    public class GetBySymbolQuery : IQuery<CurrencyResponseDto>
    {
        public string Symbol { get; set; } = string.Empty;
    }
}