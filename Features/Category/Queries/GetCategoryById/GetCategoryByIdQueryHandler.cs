using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Category;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponseDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryResponseDto>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(query.Id);
                
                if (category == null)
                {
                    return await Result<CategoryResponseDto>.FaildAsync(false, "Category not found.");
                }

                var responseDto = new CategoryResponseDto
                {
                    Id = category.Id,
                    EnglishName = category.EnglishName,
                    ArabicName = category.ArabicName,
                    DepartmentId = category.DepartmentId,
                    DepartmentName = category.Department?.EnglishName ?? string.Empty,
                    ProductsCount = category.Products?.Count ?? 0,
                    CreatedAt = category.CreatedAt,
                    LastModifiedAt = category.LastModifiedAt,
                    IsDeleted = category.IsDeleted
                };

                return await Result<CategoryResponseDto>.SuccessAsync(responseDto, "Category retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<CategoryResponseDto>.FaildAsync(false, $"Error retrieving category: {ex.Message}");
            }
        }
    }
} 