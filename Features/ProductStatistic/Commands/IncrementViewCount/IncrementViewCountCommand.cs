using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.IncrementViewCount
{
    public class IncrementViewCountCommand : ICommand<ProductStatisticResponseDto>
    {
        public int ProductId { get; set; }
    }
} 