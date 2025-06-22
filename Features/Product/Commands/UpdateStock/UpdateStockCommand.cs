using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Product.Dtos;

namespace Alwalid.Cms.Api.Features.Product.Commands.UpdateStock
{
    public class UpdateStockCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public StockRequestDto Request {  get; set; }
    }
} 