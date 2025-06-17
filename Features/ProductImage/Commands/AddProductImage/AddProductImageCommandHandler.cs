using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.AddProductImage
{
    public class AddProductImageCommandHandler : ICommandHandler<AddProductImageCommand, ProductImageResponseDto>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;

        public AddProductImageCommandHandler(
            IProductImageRepository productImageRepository,
            IProductRepository productRepository,
            IImageRepository imageRepository)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Result<ProductImageResponseDto>> Handle(AddProductImageCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(command.ProductId);
            if (product == null)
                return await Result<ProductImageResponseDto>.FaildAsync(false, "Product not found.");

            var imageUrl = await _imageRepository.Upload(product, command.Image);
            if (string.IsNullOrEmpty(imageUrl))
                return await Result<ProductImageResponseDto>.FaildAsync(false, "Image upload failed.");

            var productImage = new Entities.ProductImage
            {
                ProductId = command.ProductId,
                ImageUrl = imageUrl
            };

            var created = await _productImageRepository.CreateAsync(productImage);

            var response = new ProductImageResponseDto
            {
                Id = created.Id,
                ProductId = created.ProductId,
                ImageUrl = created.ImageUrl
            };

            return await Result<ProductImageResponseDto>.SuccessAsync(response, "Image uploaded successfully.");
        }
    }
}