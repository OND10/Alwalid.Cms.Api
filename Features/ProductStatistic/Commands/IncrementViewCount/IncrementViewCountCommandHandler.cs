using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.IncrementViewCount
{
    public class IncrementViewCountCommandHandler : ICommandHandler<IncrementViewCountCommand, ProductStatisticResponseDto>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;
        private readonly IProductRepository _productRepository;

        public IncrementViewCountCommandHandler(
            IProductStatisticRepository productStatisticRepository,
            IProductRepository productRepository)
        {
            _productStatisticRepository = productStatisticRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<ProductStatisticResponseDto>> Handle(IncrementViewCountCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.ProductId))
                {
                    return await Result<ProductStatisticResponseDto>.FaildAsync(false, "Product not found.");
                }

                var today = DateTime.Today;
                
                // Check if statistic exists for today
                var existingStatistic = await _productStatisticRepository.GetByProductAndDateAsync(command.ProductId, today);
                
                Entities.ProductStatistic statistic;
                
                if (existingStatistic != null)
                {
                    // Increment existing view count
                    existingStatistic.ViewedCounts += 1;
                    statistic = await _productStatisticRepository.UpdateAsync(existingStatistic);
                }
                else
                {
                    // Create new statistic record
                    statistic = new Entities.ProductStatistic
                    {
                        ProductId = command.ProductId,
                        ViewedCounts = 1,
                        QuantitySold = 0,
                        Date = today
                    };
                    
                    statistic = await _productStatisticRepository.CreateAsync(statistic);
                }

                // Map to response DTO
                var responseDto = new ProductStatisticResponseDto
                {
                    Id = statistic.Id,
                    ProductId = statistic.ProductId,
                    ProductName = statistic.Product?.EnglishName ?? string.Empty,
                    ViewedCounts = statistic.ViewedCounts,
                    QuantitySold = statistic.QuantitySold,
                    Date = statistic.Date
                };

                return await Result<ProductStatisticResponseDto>.SuccessAsync(responseDto, "View count incremented successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductStatisticResponseDto>.FaildAsync(false, $"Error incrementing view count: {ex.Message}");
            }
        }
    }
} 