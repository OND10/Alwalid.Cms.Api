using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetActiveCurrencies
{
    public class GetActiveCurrenciesQuery : IQuery<IEnumerable<CurrencyResponseDto>>
    {
        // No parameters needed for getting active currencies
    }
}