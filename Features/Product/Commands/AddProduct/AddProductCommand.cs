using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Microsoft.AspNetCore.Http;

namespace Alwalid.Cms.Api.Features.Product.Commands.AddProduct
{
    public class AddProductCommand : ICommand<ProductResponseDto>
    {
        public ProductRequestDto Request { get; set; } = new();
    }
} 