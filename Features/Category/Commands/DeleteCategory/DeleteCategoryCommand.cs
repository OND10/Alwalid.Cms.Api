using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 