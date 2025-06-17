using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Alwalid.Cms.Api.Features.ProductImage;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMemoryCache _memoryCache;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IImageRepository imageRepository,
            IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _imageRepository = imageRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ProductResponseDto>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                var existingProduct = await _productRepository.GetByIdAsync(command.Id);
                if (existingProduct == null)
                {
                    return await Result<ProductResponseDto>.FaildAsync(false, "Product not found.");
                }

                // Update product
                existingProduct.EnglishName = command.Request.EnglishName;
                existingProduct.ArabicName = command.Request.ArabicName;
                existingProduct.EnglishDescription = command.Request.EnglishDescription;
                existingProduct.ArabicDescription = command.Request.ArabicDescription;
                existingProduct.Price = command.Request.Price;
                existingProduct.Stock = command.Request.Stock;
                existingProduct.DepartmentId = command.Request.DepartmentId;
                existingProduct.CategoryId = command.Request.CategoryId;
                existingProduct.CurrencyId = command.Request.CurrencyId;

                var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

                // Handle image upload if provided
                if (command.Image != null)
                {
                    var imageUrl = await _imageRepository.Upload(updatedProduct, command.Image);
                    
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var productImage = new Entities.ProductImage
                        {
                            ImageUrl = imageUrl,
                            ProductId = updatedProduct.Id
                        };

                        await _productImageRepository.CreateAsync(productImage);
                    }
                }

                // Map to response DTO
                var responseDto = new ProductResponseDto
                {
                    Id = updatedProduct.Id,
                    EnglishName = updatedProduct.EnglishName,
                    ArabicName = updatedProduct.ArabicName,
                    EnglishDescription = updatedProduct.EnglishDescription,
                    ArabicDescription = updatedProduct.ArabicDescription,
                    Price = updatedProduct.Price,
                    Stock = updatedProduct.Stock,
                    DepartmentId = updatedProduct.DepartmentId,
                    DepartmentName = updatedProduct.Department?.EnglishName ?? string.Empty,
                    CategoryId = updatedProduct.CategoryId,
                    CategoryName = updatedProduct.Category?.EnglishName,
                    CurrencyId = updatedProduct.CurrencyId,
                    CurrencySymbol = updatedProduct.Currency?.Symbol,
                    ImagesCount = updatedProduct.Images?.Count ?? 0,
                    StatisticsCount = updatedProduct.Statistics?.Count ?? 0,
                    CreatedAt = updatedProduct.CreatedAt,
                    LastModifiedAt = updatedProduct.LastModifiedAt,
                    IsDeleted = updatedProduct.IsDeleted
                };

                // Clear cache for products
                _memoryCache.Remove("GetAllProducts");
                _memoryCache.Remove("GetActiveProducts");

                return await Result<ProductResponseDto>.SuccessAsync(responseDto, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result<ProductResponseDto>.FaildAsync(false, $"Error updating product: {ex.Message}");
            }
        }
    }
} 