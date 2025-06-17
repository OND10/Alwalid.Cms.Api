using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Category;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, CategoryResponseDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;

        public AddCategoryCommandHandler(ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<CategoryResponseDto>> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate unique constraints
                if (await _categoryRepository.ExistsInDepartmentAsync(command.Request.DepartmentId, command.Request.EnglishName, command.Request.ArabicName))
                {
                    return await Result<CategoryResponseDto>.FaildAsync(false, "Category already exists in this department with the same name.");
                }

                // Create new category
                var category = new Entities.Category
                {
                    EnglishName = command.Request.EnglishName,
                    ArabicName = command.Request.ArabicName,
                    DepartmentId = command.Request.DepartmentId
                };

                var createdCategory = await _categoryRepository.CreateAsync(category);

                // Map to response DTO
                var responseDto = new CategoryResponseDto
                {
                    Id = createdCategory.Id,
                    EnglishName = createdCategory.EnglishName,
                    ArabicName = createdCategory.ArabicName,
                    DepartmentId = createdCategory.DepartmentId,
                    DepartmentName = createdCategory.Department?.EnglishName ?? string.Empty,
                    ProductsCount = createdCategory.Products?.Count ?? 0,
                    CreatedAt = createdCategory.CreatedAt,
                    LastModifiedAt = createdCategory.LastModifiedAt,
                    IsDeleted = createdCategory.IsDeleted
                };

                // Clear cache for categories
                _memoryCache.Remove("GetAllCategories");
                _memoryCache.Remove("GetActiveCategories");

                return await Result<CategoryResponseDto>.SuccessAsync(responseDto, "Category created successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CategoryResponseDto>.FaildAsync(false, $"Error creating category: {ex.Message}");
            }
        }
    }
} 