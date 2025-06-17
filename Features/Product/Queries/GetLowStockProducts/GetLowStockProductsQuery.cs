using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetLowStockProducts
{
    public class GetLowStockProductsQuery : IQuery<IEnumerable<ProductResponseDto>>
    {
        public int Threshold { get; set; } = 10; // Default threshold for low stock
    }
} 