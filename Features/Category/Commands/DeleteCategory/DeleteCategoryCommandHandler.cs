using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Category;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _memoryCache;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if category exists
                if (await _categoryRepository.ExistsAsync(command.Id) is true)
                {
                    return await Result<bool>.FaildAsync(false, "Category not found.");
                }

                // Delete category
                var isDeleted = await _categoryRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    // Clear cache for categories
                    _memoryCache.Remove("GetAllCategories");
                    _memoryCache.Remove("GetActiveCategories");

                    return await Result<bool>.SuccessAsync(true, "Category deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete category.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting category: {ex.Message}");
            }
        }
    }
} 