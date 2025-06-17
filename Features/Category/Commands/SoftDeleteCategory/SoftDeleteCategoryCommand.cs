using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Category.Commands.SoftDeleteCategory
{
    public class SoftDeleteCategoryCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
} 