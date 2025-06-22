using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.AddProductStatistic
{
    public class AddProductStatisticCommandHandler : ICommandHandler<AddProductStatisticCommand, ProductStatisticResponseDto>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;
        private readonly IProductRepository _productRepository;

        public AddProductStatisticCommandHandler(
            IProductStatisticRepository productStatisticRepository,
            IProductRepository productRepository)
        {
            _productStatisticRepository = productStatisticRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<ProductStatisticResponseDto>> Handle(AddProductStatisticCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.Request.ProductId))
                {
                    return await Result<ProductStatisticResponseDto>.FaildAsync(false, "Product not found.");
                }

                // Create new product statistic
                var statistic = new Entities.ProductStatistic
                {
                    ProductId = command.Request.ProductId,
                    //Views = command.Request.Views,
                    //Sales = command.Request.Sales,
                    //Revenue = command.Request.Revenue,
                    Date = command.Request.Date,
                    QuantitySold = command.Request.QuantitySold
                };

                var createdStatistic = await _productStatisticRepository.CreateAsync(statistic);

                // Map to response DTO
                var responseDto = new ProductStatisticResponseDto
                {
                    Id = createdStatistic.Id,
                    ProductId = createdStatistic.ProductId,
                    ProductName = createdStatistic.Product?.EnglishName ?? string.Empty,
                    //Views = createdStatistic.Views,
                    //Sales = createdStatistic.Sales,
                    //Revenue = createdStatistic.Revenue,
                    Date = createdStatistic.Date,
                    //CreatedAt = createdStatistic.CreatedAt,
                    //LastModifiedAt = createdStatistic.LastModifiedAt,
                    //IsDeleted = createdStatistic.IsDeleted
                    QuantitySold = createdStatistic.QuantitySold,
                };

                return await Result<ProductStatisticResponseDto>.SuccessAsync(responseDto, "Product statistic created successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<ProductStatisticResponseDto>.FaildAsync(false, $"Error creating product statistic: {ex.Message}");
            }
        }
    }
} 