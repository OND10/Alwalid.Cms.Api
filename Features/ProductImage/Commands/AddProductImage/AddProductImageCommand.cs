using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;
using Microsoft.AspNetCore.Http;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.AddProductImage
{
    public class AddProductImageCommand : ICommand<ProductImageResponseDto>
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; } = null!;
    }
}