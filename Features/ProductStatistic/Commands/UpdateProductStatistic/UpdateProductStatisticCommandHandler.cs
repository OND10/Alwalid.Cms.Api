using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.UpdateProductStatistic
{
    public class UpdateProductStatisticCommandHandler : ICommandHandler<UpdateProductStatisticCommand, ProductStatisticResponseDto>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;
        private readonly IProductRepository _productRepository;

        public UpdateProductStatisticCommandHandler(
            IProductStatisticRepository productStatisticRepository,
            IProductRepository productRepository)
        {
            _productStatisticRepository = productStatisticRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<ProductStatisticResponseDto>> Handle(UpdateProductStatisticCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product statistic exists
                var existingStatistic = await _productStatisticRepository.GetByIdAsync(command.Id);
                if (existingStatistic == null)
                {
                    return await Result<ProductStatisticResponseDto>.FaildAsync(false, "Product statistic not found.");
                }

                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.Request.ProductId))
                {
                    return await Result<ProductStatisticResponseDto>.FaildAsync(false, "Product not found.");
                }

                // Update product statistic
                existingStatistic.ProductId = command.Request.ProductId;
                //existingStatistic.Views = command.Request.Views;
                //existingStatistic.Sales = command.Request.Sales;
                //existingStatistic.Revenue = command.Request.Revenue;
                existingStatistic.Date = command.Request.Date;
                existingStatistic.QuantitySold = command.Request.QuantitySold;
                existingStatistic.ViewedCounts = command.Request.ViewedCounts;

                var updatedStatistic = await _productStatisticRepository.UpdateAsync(existingStatistic);

                // Map to response DTO
                var responseDto = new ProductStatisticResponseDto
                {
                    Id = updatedStatistic.Id,
                    ProductId = updatedStatistic.ProductId,
                    ProductName = updatedStatistic.Product?.EnglishName ?? string.Empty,
                    //Views = updatedStatistic.Views,
                    //Sales = updatedStatistic.Sales,
                    //Revenue = updatedStatistic.Revenue,
                    Date = updatedStatistic.Date,
                    //CreatedAt = updatedStatistic.CreatedAt,
                    //LastModifiedAt = updatedStatistic.LastModifiedAt,
                    //IsDeleted = updatedStatistic.IsDeleted
                    QuantitySold = updatedStatistic.QuantitySold,
                    ViewedCounts = updatedStatistic.ViewedCounts
                };

                return await Result<ProductStatisticResponseDto>.SuccessAsync(responseDto, "Product statistic updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductStatisticResponseDto>.FaildAsync(false, $"Error updating product statistic: {ex.Message}");
            }
        }
    }
} 