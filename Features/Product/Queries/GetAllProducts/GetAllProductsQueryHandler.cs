using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetAllProducts";

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<ProductResponseDto>>? products))
                {
                    var result = await _productRepository.GetAllAsync();

                    

                    var responseDtos = result.Select(product => new ProductResponseDto
                    {
                        Id = product.Id,
                        EnglishName = product.EnglishName,
                        ArabicName = product.ArabicName,
                        EnglishDescription = product.EnglishDescription,
                        ArabicDescription = product.ArabicDescription,
                        Price = product.Price,
                        Stock = product.Stock,
                        DepartmentId = product.DepartmentId,
                        DepartmentName = product.Department?.EnglishName ?? string.Empty,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category?.EnglishName,
                        CurrencyId = product.CurrencyId,
                        CurrencySymbol = product.Currency?.Symbol,
                        ImagesCount = product.Images?.Count ?? 0,
                        StatisticsCount = product.Statistics?.Count ?? 0,
                        CreatedAt = product.CreatedAt,
                        LastModifiedAt = product.LastModifiedAt,
                        IsDeleted = product.IsDeleted,
                        ImageUrls = product.Images!.Select(i=>i.ImageUrl).ToList(),
                    });

                    products = await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(responseDtos, "Products retrieved successfully.", true);

                    if (products.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, products, cacheExpiryOptions);

                        return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(products.Data, "Products retrieved successfully.", true);
                    }
                }

                return products ?? await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, "No products found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, $"Error retrieving products: {ex.Message}");
            }
        }
    }
} 