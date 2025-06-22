using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Category;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Category.Commands.SoftDeleteCategory
{
    public class SoftDeleteCategoryCommandHandler : ICommandHandler<SoftDeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;

        public SoftDeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(SoftDeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category exists
                if (await _categoryRepository.ExistsAsync(command.Id) is false)
                {
                    return await Result<bool>.FaildAsync(false, "Category not found.");
                }

                // Soft delete category
                var isDeleted = await _categoryRepository.SoftDeleteAsync(command.Id, command.DeletedBy);

                if (isDeleted)
                {
                    // Clear cache for categories
                    _memoryCache.Remove("GetAllCategories");
                    _memoryCache.Remove("GetActiveCategories");

                    return await Result<bool>.SuccessAsync(true, "Category soft deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to soft delete category.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error soft deleting category: {ex.Message}");
            }
        }
    }
} 