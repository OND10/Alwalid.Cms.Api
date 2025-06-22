using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public DeleteProductCommandHandler(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Product not found.");
                }

                // Delete product
                var isDeleted = await _productRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    // Clear cache for products
                    _memoryCache.Remove("GetAllProducts");
                    _memoryCache.Remove("GetActiveProducts");

                    return await Result<bool>.SuccessAsync(true, "Product deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete product.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting product: {ex.Message}");
            }
        }
    }
} 