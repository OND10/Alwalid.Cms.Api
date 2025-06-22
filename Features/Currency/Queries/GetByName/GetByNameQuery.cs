using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetByName
{
    public class GetByNameQuery : IQuery<CurrencyResponseDto>
    {
        public string Name { get; set; } = string.Empty;
    }
}