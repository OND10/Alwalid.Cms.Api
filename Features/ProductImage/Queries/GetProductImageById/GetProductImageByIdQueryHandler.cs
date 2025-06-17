using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.ProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;

namespace Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById
{
    public class GetProductImageByIdQueryHandler : IQueryHandler<GetProductImageByIdQuery, ProductImageResponseDto?>
    {
        private readonly IProductImageRepository _productImageRepository;

        public GetProductImageByIdQueryHandler(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<Result<ProductImageResponseDto?>> Handle(GetProductImageByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _productImageRepository.GetByIdAsync(query.Id);
                
                if (image == null)
                {
                    return await Result<ProductImageResponseDto?>.FaildAsync(false, "Product image not found.");
                }

                var responseDto = new ProductImageResponseDto
                {
                    Id = image.Id,
                    ProductId = image.ProductId,
                    ImageUrl = image.ImageUrl,
                    ProductName = image.Product?.EnglishName ?? string.Empty,
                    //CreatedAt = image.CreatedAt,
                    //LastModifiedAt = image.LastModifiedAt,
                    //IsDeleted = image.IsDeleted
                };

                return await Result<ProductImageResponseDto?>.SuccessAsync(responseDto, "Product image retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<ProductImageResponseDto?>.FaildAsync(false, $"Error retrieving product image: {ex.Message}");
            }
        }
    }
} 