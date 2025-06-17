using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Product.Commands.UpdateStock
{
    public class UpdateStockCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public int NewStock { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
} 