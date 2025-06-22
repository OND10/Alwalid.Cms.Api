using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Currency;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency
{
    public class DeleteCurrencyCommandHandler : ICommandHandler<DeleteCurrencyCommand, bool>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMemoryCache _memoryCache;

        public DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository, IMemoryCache memoryCache)
        {
            _currencyRepository = currencyRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<bool>> Handle(DeleteCurrencyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if currency exists
                if (!await _currencyRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Currency not found.");
                }

                // Delete currency
                var isDeleted = await _currencyRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    // Clear cache for currencies
                    _memoryCache.Remove("GetAllCurrencies");
                    _memoryCache.Remove("GetActiveCurrencies");

                    return await Result<bool>.SuccessAsync(true, "Currency deleted successfully.", true);
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete currency.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting currency: {ex.Message}");
            }
        }
    }
} 