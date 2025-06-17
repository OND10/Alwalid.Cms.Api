using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.SearchProducts
{
    public class SearchProductsQuery : IQuery<IEnumerable<ProductResponseDto>>
    {
        public string SearchTerm { get; set; }
    }
} 