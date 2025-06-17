using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetProductStatisticById
{
    public class GetProductStatisticByIdQuery : IQuery<ProductStatisticResponseDto?>
    {
        public int Id { get; set; }
    }
} 