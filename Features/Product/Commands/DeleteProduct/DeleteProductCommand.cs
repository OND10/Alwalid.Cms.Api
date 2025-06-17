using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 