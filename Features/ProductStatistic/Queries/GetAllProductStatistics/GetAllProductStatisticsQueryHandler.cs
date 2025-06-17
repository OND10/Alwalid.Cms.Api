using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetAllProductStatistics
{
    public class GetAllProductStatisticsQueryHandler : IQueryHandler<GetAllProductStatisticsQuery, IEnumerable<ProductStatisticResponseDto>>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetAllProductStatistics";

        public GetAllProductStatisticsQueryHandler(IProductStatisticRepository productStatisticRepository, IMemoryCache memoryCache)
        {
            _productStatisticRepository = productStatisticRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<ProductStatisticResponseDto>>> Handle(GetAllProductStatisticsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<ProductStatisticResponseDto>>? statistics))
                {
                    var result = await _productStatisticRepository.GetAllAsync();

                    var responseDtos = result.Select(statistic => new ProductStatisticResponseDto
                    {
                        Id = statistic.Id,
                        ProductId = statistic.ProductId,
                        ProductName = statistic.Product?.EnglishName ?? string.Empty,
                        //Views = statistic.Views,
                        //Sales = statistic.Sales,
                        //Revenue = statistic.Revenue,
                        Date = statistic.Date,
                        //CreatedAt = statistic.CreatedAt,
                        //LastModifiedAt = statistic.LastModifiedAt,
                        //IsDeleted = statistic.IsDeleted
                    });

                    statistics = await Result<IEnumerable<ProductStatisticResponseDto>>.SuccessAsync(responseDtos, "Product statistics retrieved successfully.");

                    if (statistics.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, statistics, cacheExpiryOptions);

                        return await Result<IEnumerable<ProductStatisticResponseDto>>.SuccessAsync(statistics.Data, "Product statistics retrieved successfully.", true);
                    }
                }

                return statistics ?? await Result<IEnumerable<ProductStatisticResponseDto>>.FaildAsync(false, "No product statistics found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductStatisticResponseDto>>.FaildAsync(false, $"Error retrieving product statistics: {ex.Message}");
            }
        }
    }
} 