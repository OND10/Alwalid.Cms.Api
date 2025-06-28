using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetMostViewedProducts
{
    public class GetMostViewedProductsQueryHandler : IQueryHandler<GetMostViewedProductsQuery, IEnumerable<ProductStatisticResponseDto>>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public GetMostViewedProductsQueryHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<IEnumerable<ProductStatisticResponseDto>>> Handle(GetMostViewedProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productStatisticRepository.GetMostViewedProductsAsync(
                    query.Request.Top, 
                    query.Request.StartDate, 
                    query.Request.EndDate);

                var responseDtos = result.Select(statistic => new ProductStatisticResponseDto
                {
                    Id = statistic.Id,
                    ProductId = statistic.ProductId,
                    ProductName = statistic.Product?.EnglishName ?? string.Empty,
                    ViewedCounts = statistic.ViewedCounts,
                    QuantitySold = statistic.QuantitySold,
                    Date = statistic.Date
                });

                return await Result<IEnumerable<ProductStatisticResponseDto>>.SuccessAsync(responseDtos, "Most viewed products retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductStatisticResponseDto>>.FaildAsync(false, $"Error retrieving most viewed products: {ex.Message}");
            }
        }
    }
} 