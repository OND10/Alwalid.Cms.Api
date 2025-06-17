using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByProductId
{
    public class GetByProductIdForStatisticQuery : IQuery<IEnumerable<ProductStatisticResponseDto>>
    {
        public int ProductId { get; set; }
    }
} 