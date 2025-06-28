using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetMostViewedProducts
{
    public class GetMostViewedProductsQuery : IQuery<IEnumerable<ProductStatisticResponseDto>>
    {
        public MostViewedProductDto Request { get; set; } = new();
    }
} 