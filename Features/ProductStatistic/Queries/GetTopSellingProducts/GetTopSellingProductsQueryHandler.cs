using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts
{
    public class GetTopSellingProductsQueryHandler : IQueryHandler<GetTopSellingProductsQuery, IEnumerable<ProductStatisticResponseDto>>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public GetTopSellingProductsQueryHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<IEnumerable<ProductStatisticResponseDto>>> Handle(GetTopSellingProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productStatisticRepository.GetTopSellingProductsAsync(query.Top, query.startDate, query.endDate);

                var responseDtos = result.Select(statistic => new ProductStatisticResponseDto
                {
                    Id = statistic.Id,
                    ProductId = statistic.ProductId,
                    ProductName = statistic.Product?.EnglishName ?? string.Empty,
                    //Views = statistic.Views,
                    //Sales = statistic.Sales,
                    //Revenue = statistic.Revenue,
                    //Date = statistic.Date,
                    //CreatedAt = statistic.CreatedAt,
                    //LastModifiedAt = statistic.LastModifiedAt,
                    //IsDeleted = statistic.IsDeleted
                });

                return await Result<IEnumerable<ProductStatisticResponseDto>>.SuccessAsync(responseDtos, "Top selling products retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductStatisticResponseDto>>.FaildAsync(false, $"Error retrieving top selling products: {ex.Message}");
            }
        }
    }
} 