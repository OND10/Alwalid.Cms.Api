using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponseDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductResponseDto?>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(query.Id);
                
                if (product == null)
                {
                    return await Result<ProductResponseDto?>.FaildAsync(false, "Product not found.");
                }

                var responseDto = new ProductResponseDto
                {
                    Id = product.Id,
                    EnglishName = product.EnglishName,
                    ArabicName = product.ArabicName,
                    EnglishDescription = product.EnglishDescription,
                    ArabicDescription = product.ArabicDescription,
                    Price = product.Price,
                    Stock = product.Stock,
                    DepartmentId = product.DepartmentId,
                    DepartmentName = product.Department?.EnglishName ?? string.Empty,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category?.EnglishName,
                    CurrencyId = product.CurrencyId,
                    CurrencySymbol = product.Currency?.Symbol,
                    ImagesCount = product.Images?.Count ?? 0,
                    StatisticsCount = product.Statistics?.Count ?? 0,
                    CreatedAt = product.CreatedAt,
                    LastModifiedAt = product.LastModifiedAt,
                    IsDeleted = product.IsDeleted
                };

                return await Result<ProductResponseDto?>.SuccessAsync(responseDto, "Product retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<ProductResponseDto?>.FaildAsync(false, $"Error retrieving product: {ex.Message}");
            }
        }
    }
} 