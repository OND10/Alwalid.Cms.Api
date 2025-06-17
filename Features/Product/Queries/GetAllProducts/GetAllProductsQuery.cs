using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IQuery<IEnumerable<ProductResponseDto>>
    {
        // No parameters needed for getting all products
    }
} 