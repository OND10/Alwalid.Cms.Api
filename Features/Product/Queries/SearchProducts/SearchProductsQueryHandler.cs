using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;

        public SearchProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(SearchProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productRepository.SearchProductsAsync(query.SearchTerm);

                var responseDtos = result.Select(product => new ProductResponseDto
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
                });

                return await Result<IEnumerable<ProductResponseDto>>.SuccessAsync(responseDtos, "Products search completed successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductResponseDto>>.FaildAsync(false, $"Error searching products: {ex.Message}");
            }
        }
    }
}