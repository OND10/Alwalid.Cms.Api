using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById
{
    public class GetCurrencyByIdQuery : IQuery<CurrencyResponseDto?>
    {
        public int Id { get; set; }
    }
} 