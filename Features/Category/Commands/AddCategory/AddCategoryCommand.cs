using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Commands.AddCategory
{
    public class AddCategoryCommand : ICommand<CategoryResponseDto>
    {
        public CategoryRequestDto Request { get; set; } = new();
    }
} 