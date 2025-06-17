using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : ICommand<CategoryResponseDto>
    {
        public int Id { get; set; }
        public CategoryRequestDto Request { get; set; } = new();
    }
} 