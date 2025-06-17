using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Product.Commands.SoftDeleteProduct
{
    public class SoftDeleteProductCommandHandler : ICommandHandler<SoftDeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;

        public SoftDeleteProductCommandHandler(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(SoftDeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product exists
                if (!await _productRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Product not found.");
                }

                // Soft delete product
                var isDeleted = await _productRepository.SoftDeleteAsync(command.Id, command.DeletedBy);

                if (isDeleted)
                {
                    // Clear cache for products
                    _memoryCache.Remove("GetAllProducts");
                    _memoryCache.Remove("GetActiveProducts");

                    return await Result<bool>.SuccessAsync(true, "Product soft deleted successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to soft delete product.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error soft deleting product: {ex.Message}");
            }
        }
    }
} 