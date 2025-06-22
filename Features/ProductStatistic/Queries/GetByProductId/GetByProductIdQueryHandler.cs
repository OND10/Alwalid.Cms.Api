using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByProductId
{
    public class GetByProductIdForStatisticQueryHandler : IQueryHandler<GetByProductIdForStatisticQuery, IEnumerable<ProductStatisticResponseDto>>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public GetByProductIdForStatisticQueryHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<IEnumerable<ProductStatisticResponseDto>>> Handle(GetByProductIdForStatisticQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productStatisticRepository.GetByProductIdAsync(query.ProductId);

                var responseDtos = result.Select(statistic => new ProductStatisticResponseDto
                {
                    Id = statistic.Id,
                    ProductId = statistic.ProductId,
                    ProductName = statistic.Product?.EnglishName ?? string.Empty,
                    Date = statistic.Date,
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