using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts
{
    public class GetTopSellingProductsQuery : IQuery<IEnumerable<ProductStatisticResponseDto>>
    {
            public TopSellingProductDto Request { get; set; }
    }
}