using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetByProductId
{
    public class GetByProductIdQueryHandler : IQueryHandler<GetByProductIdQuery, IEnumerable<ProductImageResponseDto>>
    {
        private readonly IProductImageRepository _productImageRepository;

        public GetByProductIdQueryHandler(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<Result<IEnumerable<ProductImageResponseDto>>> Handle(GetByProductIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _productImageRepository.GetByProductIdAsync(query.ProductId);

                var responseDtos = result.Select(image => new ProductImageResponseDto
                {
                    Id = image.Id,
                    ProductId = image.ProductId,
                    ImageUrl = image.ImageUrl,
                    ProductName = image.Product?.EnglishName ?? string.Empty,
                    //CreatedAt = image.CreatedAt,
                    //LastModifiedAt = image.LastModifiedAt,
                    //IsDeleted = image.IsDeleted
                });

                return await Result<IEnumerable<ProductImageResponseDto>>.SuccessAsync(responseDtos, "Product images retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<ProductImageResponseDto>>.FaildAsync(false, $"Error retrieving product images: {ex.Message}");
            }
        }
    }
} 