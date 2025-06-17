using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency
{
    public class UpdateCurrencyCommandHandler : ICommandHandler<UpdateCurrencyCommand, CurrencyResponseDto>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMemoryCache _memoryCache;

        public UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository, IMemoryCache memoryCache)
        {
            _currencyRepository = currencyRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<CurrencyResponseDto>> Handle(UpdateCurrencyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if currency exists
                var existingCurrency = await _currencyRepository.GetByIdAsync(command.Id);
                if (existingCurrency == null)
                {
                    return await Result<CurrencyResponseDto>.FaildAsync(false, "Currency not found.");
                }

                // Validate unique constraints
                if (await _currencyRepository.ExistsByCodeAsync(command.Request.Code))
                {
                    return await Result<CurrencyResponseDto>.FaildAsync(false, "Currency with this code already exists.");
                }

                // Update currency
                existingCurrency.Name = command.Request.Name;
                existingCurrency.Symbol = command.Request.Symbol;
                existingCurrency.Code = command.Request.Code;
                //existingCurrency.ExchangeRate = command.Request.ExchangeRate;
                //existingCurrency.IsDefault = command.Request.IsDefault;
                //existingCurrency.IsActive = command.Request.IsActive;

                var updatedCurrency = await _currencyRepository.UpdateAsync(existingCurrency);

                // Map to response DTO
                var responseDto = new CurrencyResponseDto
                {
                    Id = updatedCurrency.Id,
                    Name = updatedCurrency.Name,
                    Symbol = updatedCurrency.Symbol,
                    Code = updatedCurrency.Code,
                    //ExchangeRate = updatedCurrency.ExchangeRate,
                    //IsDefault = updatedCurrency.IsDefault,
                    //IsActive = updatedCurrency.IsActive,
                    ProductsCount = updatedCurrency.Products?.Count ?? 0,
                    //CreatedAt = updatedCurrency.CreatedAt,
                    //LastModifiedAt = updatedCurrency.LastModifiedAt,
                    //IsDeleted = updatedCurrency.IsDeleted
                };

                // Clear cache for currencies
                _memoryCache.Remove("GetAllCurrencies");
                _memoryCache.Remove("GetActiveCurrencies");

                return await Result<CurrencyResponseDto>.SuccessAsync(responseDto, "Currency updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CurrencyResponseDto>.FaildAsync(false, $"Error updating currency: {ex.Message}");
            }
        }
    }
} 