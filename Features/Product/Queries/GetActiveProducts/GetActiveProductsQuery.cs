using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetActiveProducts
{
    public class GetActiveProductsQuery : IQuery<IEnumerable<ProductResponseDto>>
    {
        // No parameters needed for getting active products
    }
} 