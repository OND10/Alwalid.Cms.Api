using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IQuery<ProductResponseDto?>
    {
        public int Id { get; set; }
    }
} 