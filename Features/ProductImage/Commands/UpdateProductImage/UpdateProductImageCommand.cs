using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;
using Microsoft.AspNetCore.Http;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.UpdateProductImage
{
    public class UpdateProductImageCommand : ICommand<ProductImageResponseDto>
    {
        public int Id { get; set; }
        public ProductImageRequestDto Request { get; set; } = new();
      
    }
} 