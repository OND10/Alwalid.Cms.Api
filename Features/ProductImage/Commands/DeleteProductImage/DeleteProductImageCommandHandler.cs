using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.ProductImage;

namespace Alwalid.Cms.Api.Features.ProductImage.Commands.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : ICommandHandler<DeleteProductImageCommand, bool>
    {
        private readonly IProductImageRepository _productImageRepository;

        public DeleteProductImageCommandHandler(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<Result<bool>> Handle(DeleteProductImageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product image exists
                if (!await _productImageRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Product image not found.");
                }

                // Delete product image
                var isDeleted = await _productImageRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Product image deleted successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete product image.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting product image: {ex.Message}");
            }
        }
    }
} 