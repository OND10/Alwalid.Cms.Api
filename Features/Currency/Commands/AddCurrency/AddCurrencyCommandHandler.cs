using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency
{
    public class AddCurrencyCommandHandler : ICommandHandler<AddCurrencyCommand, CurrencyResponseDto>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMemoryCache _memoryCache;

        public AddCurrencyCommandHandler(ICurrencyRepository currencyRepository, IMemoryCache memoryCache)
        {
            _currencyRepository = currencyRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<CurrencyResponseDto>> Handle(AddCurrencyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate unique constraints
                if (await _currencyRepository.ExistsByCodeAsync(command.Request.Code))
                {
                    return await Result<CurrencyResponseDto>.FaildAsync(false, "Currency with this code already exists.");
                }

                // Create new currency
                var currency = new Entities.Currency
                {
                    Name = command.Request.Name,
                    Symbol = command.Request.Symbol,
                    Code = command.Request.Code,
                    //ExchangeRate = command.Request.ExchangeRate,
                    //IsDefault = command.Request.IsDefault,
                    //IsActive = command.Request.IsActive
                };

                var createdCurrency = await _currencyRepository.CreateAsync(currency);

                // Map to response DTO
                var responseDto = new CurrencyResponseDto
                {
                    Id = createdCurrency.Id,
                    Name = createdCurrency.Name,
                    Symbol = createdCurrency.Symbol,
                    Code = createdCurrency.Code,
                    //ExchangeRate = createdCurrency.ExchangeRate,
                    //IsDefault = createdCurrency.IsDefault,
                    //IsActive = createdCurrency.IsActive,
                    ProductsCount = createdCurrency.Products?.Count ?? 0,
                    //CreatedAt = createdCurrency.CreatedAt,
                    //LastModifiedAt = createdCurrency.LastModifiedAt,
                    //IsDeleted = createdCurrency.IsDeleted
                };

                // Clear cache for currencies
                _memoryCache.Remove("GetAllCurrencies");
                _memoryCache.Remove("GetActiveCurrencies");

                return await Result<CurrencyResponseDto>.SuccessAsync(responseDto, "Currency created successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CurrencyResponseDto>.FaildAsync(false, $"Error creating currency: {ex.Message}");
            }
        }
    }
} 