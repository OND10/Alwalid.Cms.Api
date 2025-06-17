using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.AddProductStatistic
{
    public class AddProductStatisticCommand : ICommand<ProductStatisticResponseDto>
    {
        public ProductStatisticRequestDto Request { get; set; } = new();
    }
} 