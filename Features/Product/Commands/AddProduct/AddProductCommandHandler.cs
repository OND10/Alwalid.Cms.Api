using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Alwalid.Cms.Api.Features.ProductImage;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Commands.AddProduct
{
    public class AddProductCommandHandler : ICommandHandler<AddProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMemoryCache _memoryCache;

        public AddProductCommandHandler(
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

        public async Task<Result<ProductResponseDto>> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Create new product
                var product = new Entities.Product
                {
                    EnglishName = command.Request.EnglishName,
                    ArabicName = command.Request.ArabicName,
                    EnglishDescription = command.Request.EnglishDescription,
                    ArabicDescription = command.Request.ArabicDescription,
                    Price = command.Request.Price,
                    Stock = command.Request.Stock,
                    DepartmentId = command.Request.DepartmentId,
                    CategoryId = command.Request.CategoryId,
                    CurrencyId = command.Request.CurrencyId
                };

                var createdProduct = await _productRepository.CreateAsync(product);

                // Handle image upload if provided
                if (command.Request.Image != null)
                {
                    var imageUrl = await _imageRepository.Upload(product, command.Request.Image);
                    
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var productImage = new Entities.ProductImage
                        {
                            ImageUrl = imageUrl,
                            ProductId = createdProduct.Id
                        };

                        await _productImageRepository.CreateAsync(productImage);
                    }
                }

                // Map to response DTO
                var responseDto = new ProductResponseDto
                {
                    Id = createdProduct.Id,
                    EnglishName = createdProduct.EnglishName,
                    ArabicName = createdProduct.ArabicName,
                    EnglishDescription = createdProduct.EnglishDescription,
                    ArabicDescription = createdProduct.ArabicDescription,
                    Price = createdProduct.Price,
                    Stock = createdProduct.Stock,
                    DepartmentId = createdProduct.DepartmentId,
                    DepartmentName = createdProduct.Department?.EnglishName ?? string.Empty,
                    CategoryId = createdProduct.CategoryId,
                    CategoryName = createdProduct.Category?.EnglishName,
                    CurrencyId = createdProduct.CurrencyId,
                    CurrencySymbol = createdProduct.Currency?.Symbol,
                    ImagesCount = createdProduct.Images?.Count ?? 0,
                    StatisticsCount = createdProduct.Statistics?.Count ?? 0,
                    CreatedAt = createdProduct.CreatedAt,
                    LastModifiedAt = createdProduct.LastModifiedAt,
                    IsDeleted = createdProduct.IsDeleted
                };

                // Clear cache for products
                _memoryCache.Remove("GetAllProducts");
                _memoryCache.Remove("GetActiveProducts");

                return await Result<ProductResponseDto>.SuccessAsync(responseDto, "Product created successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductResponseDto>.FaildAsync(false, $"Error creating product: {ex.Message}");
            }
        }
    }
} 