using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.GetLowStockProducts
{
    public class GetLowStockProductsQueryHandler : IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductResponseDto>>
    {

        private readonly IProductRepository _productRepository;

        public GetLowStockProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetLowStockProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _productRepository.GetLowStockProductsAsync(query.Threshold);
                
                if (product == null)
                {
                    return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, "Product not found.");
                }

                var responseDto = product.Select(x => new ProductResponseDto
                {
                    Id = x.Id,
                    EnglishName = x.EnglishName,
                    ArabicName = x.ArabicName,
                    EnglishDescription = x.EnglishDescription,
                    ArabicDescription = x.ArabicDescription,
                    Price = x.Price,
                    Stock = x.Stock,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department?.EnglishName ?? string.Empty,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category?.EnglishName,
                    CurrencyId = x.CurrencyId,
                    CurrencySymbol = x.Currency?.Symbol,
                    ImagesCount = x.Images?.Count ?? 0,
                    StatisticsCount = x.Statistics?.Count ?? 0,
                    CreatedAt = x.CreatedAt,
                    LastModifiedAt = x.LastModifiedAt,
                    IsDeleted = x.IsDeleted
                });

                return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(responseDto, "Product retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, $"Error retrieving product: {ex.Message}");
            }
        }
    }
} 