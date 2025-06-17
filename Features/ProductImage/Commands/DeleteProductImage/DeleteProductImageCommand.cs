using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.DeleteProductImage
{
    public class DeleteProductImageCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 