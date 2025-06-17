using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetAllProductStatistics
{
    public class GetAllProductStatisticsQuery : IQuery<IEnumerable<ProductStatisticResponseDto>>
    {
        // No parameters needed for getting all product statistics
    }
} 