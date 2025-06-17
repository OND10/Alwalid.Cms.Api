using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.ProductStatistic;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.DeleteProductStatistic
{
    public class DeleteProductStatisticCommandHandler : ICommandHandler<DeleteProductStatisticCommand, bool>
    {
        private readonly IProductStatisticRepository _productStatisticRepository;

        public DeleteProductStatisticCommandHandler(IProductStatisticRepository productStatisticRepository)
        {
            _productStatisticRepository = productStatisticRepository;
        }

        public async Task<Result<bool>> Handle(DeleteProductStatisticCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if product statistic exists
                if (!await _productStatisticRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Product statistic not found.");
                }

                // Delete product statistic
                var isDeleted = await _productStatisticRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Product statistic deleted successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete product statistic.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting product statistic: {ex.Message}");
            }
        }
    }
} 