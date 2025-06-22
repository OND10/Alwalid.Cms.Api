using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetProductStatisticById
{
    public class GetProductStatisticByIdQueryHandler : IQueryHandler<GetProductStatisticByIdQuery, ProductStatisticResponseDto>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public GetProductStatisticByIdQueryHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<ProductStatisticResponseDto>> Handle(GetProductStatisticByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var statistic = await _productStatisticRepository.GetByIdAsync(query.Id);

                if (statistic == null)
                {
                    return await Result<ProductStatisticResponseDto>.FaildAsync(false, "Product statistic not found.");
                }

                var responseDto = new ProductStatisticResponseDto
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
                };

                return await Result<ProductStatisticResponseDto>.SuccessAsync(responseDto, "Product statistic retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductStatisticResponseDto>.FaildAsync(false, $"Error retrieving product statistic: {ex.Message}");
            }
        }
    }
}