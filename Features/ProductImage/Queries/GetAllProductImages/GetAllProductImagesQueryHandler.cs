using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetAllProductImages
{
    public class GetAllProductImagesQueryHandler : IQueryHandler<GetAllProductImagesQuery, IEnumerable<ProductImageResponseDto>>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetAllProductImages";

        public GetAllProductImagesQueryHandler(IProductImageRepository productImageRepository, IMemoryCache memoryCache)
        {
            _productImageRepository = productImageRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<ProductImageResponseDto>>> Handle(GetAllProductImagesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<ProductImageResponseDto>>? images))
                {
                    var result = await _productImageRepository.GetAllAsync();

                    var responseDtos = result.Select(image => new ProductImageResponseDto
                    {
                        Id = image.Id,
                        ProductId = image.ProductId,
                        ImageUrl = image.ImageUrl,
                        ProductName = image.Product?.EnglishName ?? string.Empty,
                        //CreatedAt = image.CreatedAt,
                        //LastModifiedAt = image.LastModifiedAt,
                        //IsDeleted = image.IsDeleted
                    });

                    images = await Result<IEnumerable<ProductImageResponseDto>>.SuccessAsync(responseDtos, "Product images retrieved successfully.");

                    if (images.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, images, cacheExpiryOptions);

                        return await Result<IEnumerable<ProductImageResponseDto>>.SuccessAsync(images.Data, "Product images retrieved successfully.", true);
                    }
                }

                return images ?? await Result<IEnumerable<ProductImageResponseDto>>.FaildAsync(false, "No product images found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductImageResponseDto>>.FaildAsync(false, $"Error retrieving product images: {ex.Message}");
            }
        }
    }
} 