using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.UpdateProductStatistic
{
    public class UpdateProductStatisticCommand : ICommand<ProductStatisticResponseDto>
    {
        public int Id { get; set; }
        public ProductStatisticRequestDto Request { get; set; } = new();
    }
} 