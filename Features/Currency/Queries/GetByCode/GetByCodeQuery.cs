using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetByCode
{
    public class GetByCodeQuery : IQuery<CurrencyResponseDto?>
    {
        public string Code { get; set; } = string.Empty;
    }
} 