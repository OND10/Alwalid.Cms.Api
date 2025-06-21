using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Category;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponseDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<CategoryResponseDto>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category exists
                var existingCategory = await _categoryRepository.GetByIdAsync(command.Id);
                if (existingCategory == null)
                {
                    return await Result<CategoryResponseDto>.FaildAsync(false, "Category not found.");
                }

                // Validate unique constraints
                if (await _categoryRepository.ExistsInDepartmentAsync(command.Request.DepartmentId, command.Request.EnglishName, command.Request.ArabicName, command.Id) is true)
                {
                    return await Result<CategoryResponseDto>.FaildAsync(false, "Category already exists in this department with the same name.");
                }

                // Update category
                existingCategory.EnglishName = command.Request.EnglishName;
                existingCategory.ArabicName = command.Request.ArabicName;
                existingCategory.DepartmentId = command.Request.DepartmentId;

                var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);

                // Map to response DTO
                var responseDto = new CategoryResponseDto
                {
                    Id = updatedCategory.Id,
                    EnglishName = updatedCategory.EnglishName,
                    ArabicName = updatedCategory.ArabicName,
                    DepartmentId = updatedCategory.DepartmentId,
                    DepartmentName = updatedCategory.Department?.EnglishName ?? string.Empty,
                    ProductsCount = updatedCategory.Products?.Count ?? 0,
                    CreatedAt = updatedCategory.CreatedAt,
                    LastModifiedAt = updatedCategory.LastModifiedAt,
                    IsDeleted = updatedCategory.IsDeleted
                };

                // Clear cache for categories
                _memoryCache.Remove("GetAllCategories");
                _memoryCache.Remove("GetActiveCategories");

                return await Result<CategoryResponseDto>.SuccessAsync(responseDto, "Category updated successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<CategoryResponseDto>.FaildAsync(false, $"Error updating category: {ex.Message}");
            }
        }
    }
} 