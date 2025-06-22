using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Commands.UpdateStock
{
    public class UpdateStockCommandHandler : ICommandHandler<UpdateStockCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public UpdateStockCommandHandler(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(UpdateStockCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Product not found.");
                }

                // Update stock
                var isUpdated = await _productRepository.UpdateStockAsync(command.Id, command.Request.NewStock);

                if (isUpdated)
                {
                    // Clear cache for products
                    _memoryCache.Remove("GetAllProducts");
                    _memoryCache.Remove("GetActiveProducts");

                    return await Result<bool>.SuccessAsync(true, "Product stock updated successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to update product stock.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error updating product stock: {ex.Message}");
            }
        }
    }
}