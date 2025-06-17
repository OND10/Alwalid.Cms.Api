using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById
{
    public class GetProductImageByIdQuery : IQuery<ProductImageResponseDto?>
    {
        public int Id { get; set; }
    }
} 