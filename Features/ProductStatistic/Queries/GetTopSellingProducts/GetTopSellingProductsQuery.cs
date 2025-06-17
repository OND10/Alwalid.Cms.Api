using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts
{
    public class GetTopSellingProductsQuery : IQuery<IEnumerable<ProductStatisticResponseDto>>
    {
        public int Top { get; set; } = 10;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}