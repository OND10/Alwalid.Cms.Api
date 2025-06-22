using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Microsoft.AspNetCore.Http;

namespace Alwalid.Cms.Api.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : ICommand<ProductResponseDto>
    {
        public int Id { get; set; }
        public ProductRequestDto Request { get; set; } = new();
        //public IFormFile? Image { get; set; }
    }
} 