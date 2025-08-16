using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommand : ICommand<CategoryResponseDto>, IInvalidateCacheCommand
    {
        public CategoryRequestDto Request { get; set; } = new();
        public IEnumerable<string> CacheKeys => new[] { "GetAllCategories" };

    }
} 