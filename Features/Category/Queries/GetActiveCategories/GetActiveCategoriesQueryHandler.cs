using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Category;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetActiveCategories
{
    public class GetActiveCategoriesQueryHandler : IQueryHandler<GetActiveCategoriesQuery, IEnumerable<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetActiveCategories";

        public GetActiveCategoriesQueryHandler(ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<CategoryResponseDto>>> Handle(GetActiveCategoriesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<CategoryResponseDto>>? categories))
                {
                    var result = await _categoryRepository.GetActiveCategoriesAsync();

                    var responseDtos = result.Select(category => new CategoryResponseDto
                    {
                        Id = category.Id,
                        EnglishName = category.EnglishName,
                        ArabicName = category.ArabicName,
                        DepartmentId = category.DepartmentId,
                        DepartmentName = category.Department?.EnglishName ?? string.Empty,
                        ProductsCount = category.Products?.Count ?? 0,
                        CreatedAt = category.CreatedAt,
                        LastModifiedAt = category.LastModifiedAt,
                        IsDeleted = category.IsDeleted
                    });

                    categories = await Result<IEnumerable<CategoryResponseDto>>.SuccessAsync(responseDtos, "Active categories retrieved successfully.", true);

                    if (categories.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, categories, cacheExpiryOptions);

                        return await Result<IEnumerable<CategoryResponseDto>>.SuccessAsync(categories.Data, "Active categories retrieved successfully.", true);
                    }
                }

                return categories ?? await Result<IEnumerable<CategoryResponseDto>>.FaildAsync(false, "No active categories found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<CategoryResponseDto>>.FaildAsync(false, $"Error retrieving active categories: {ex.Message}");
            }
        }
    }
} 