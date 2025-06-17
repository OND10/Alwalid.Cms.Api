using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetActiveProducts
{
    public class GetActiveProductsQueryHandler : IQueryHandler<GetActiveProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetActiveProducts";

        public GetActiveProductsQueryHandler(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetActiveProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<ProductResponseDto>>? products))
                {
                    var result = await _productRepository.GetActiveProductsAsync();

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
                        IsDeleted = product.IsDeleted
                    });

                    products = await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(responseDtos, "Active products retrieved successfully.");

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

                        return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(products.Data, "Active products retrieved successfully.", true);
                    }
                }

                return products ?? await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, "No active products found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, $"Error retrieving active products: {ex.Message}");
            }
        }
    }
} 