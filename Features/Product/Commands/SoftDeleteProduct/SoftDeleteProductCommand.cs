using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Product.Commands.SoftDeleteProduct
{
    public class SoftDeleteProductCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
} 