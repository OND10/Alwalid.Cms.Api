using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetAllProductImages
{
    public class GetAllProductImagesQuery : IQuery<IEnumerable<ProductImageResponseDto>>
    {
        // No parameters needed for getting all product images
    }
} 