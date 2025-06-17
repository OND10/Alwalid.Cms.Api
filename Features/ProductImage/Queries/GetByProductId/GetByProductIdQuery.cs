using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetByProductId
{
    public class GetByProductIdQuery : IQuery<IEnumerable<ProductImageResponseDto>>
    {
        public int ProductId { get; set; }
    }
} 