using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.ProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.UpdateProductImage
{
    public class UpdateProductImageCommandHandler : ICommandHandler<UpdateProductImageCommand, ProductImageResponseDto>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;

        public UpdateProductImageCommandHandler(
            IProductImageRepository productImageRepository,
            IProductRepository productRepository,
            IImageRepository imageRepository)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Result<ProductImageResponseDto>> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product image exists
                var existingImage = await _productImageRepository.GetByIdAsync(command.Id);
                if (existingImage == null)
                {
                    return await Result<ProductImageResponseDto>.FaildAsync(false, "Product image not found.");
                }

                // Check if product exists
                var product = await _productRepository.GetByIdAsync(command.Request.ProductId);
                if (product == null)
                {
                    return await Result<ProductImageResponseDto>.FaildAsync(false, "Product not found.");
                }

                // Handle image upload if provided
                if (command.Request.image != null)
                {
                    var imageUrl = await _imageRepository.Upload(product, command.Request.image);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        existingImage.ImageUrl = imageUrl;
                    }
                }

                // Update other properties
                existingImage.ProductId = command.Request.ProductId;

                var updatedImage = await _productImageRepository.UpdateAsync(existingImage);

                // Map to response DTO
                var responseDto = new ProductImageResponseDto
                {
                    Id = updatedImage.Id,
                    ProductId = updatedImage.ProductId,
                    ImageUrl = updatedImage.ImageUrl,
                    //CreatedAt = updatedImage.CreatedAt,
                    //LastModifiedAt = updatedImage.LastModifiedAt,
                    //IsDeleted = updatedImage.IsDeleted
                };

                return await Result<ProductImageResponseDto>.SuccessAsync(responseDto, "Product image updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductImageResponseDto>.FaildAsync(false, $"Error updating product image: {ex.Message}");
            }
        }
    }
} 