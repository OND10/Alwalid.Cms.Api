using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByDateRange
{
    public class GetByDateRangeQueryHandler : IQueryHandler<GetByDateRangeQuery, IEnumerable<ProductStatisticResponseDto>>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public GetByDateRangeQueryHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<IEnumerable<ProductStatisticResponseDto>>> Handle(GetByDateRangeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productStatisticRepository.GetByDateRangeAsync(query.StartDate, query.EndDate);

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
                    QuantitySold = statistic.QuantitySold
                });

                return await Result<IEnumerable<ProductStatisticResponseDto>>.SuccessAsync(responseDtos, "Product statistics retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductStatisticResponseDto>>.FaildAsync(false, $"Error retrieving product statistics: {ex.Message}");
            }
        }
    }
} 