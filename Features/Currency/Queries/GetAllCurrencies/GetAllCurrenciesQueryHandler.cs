using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies
{
    public class GetAllCurrenciesQueryHandler : IQueryHandler<GetAllCurrenciesQuery, IEnumerable<CurrencyResponseDto>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetAllCurrencies";

        public GetAllCurrenciesQueryHandler(ICurrencyRepository currencyRepository, IMemoryCache memoryCache)
        {
            _currencyRepository = currencyRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result<IEnumerable<CurrencyResponseDto>>> Handle(GetAllCurrenciesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKey, out Result<IEnumerable<CurrencyResponseDto>>? currencies))
                {
                    var result = await _currencyRepository.GetAllAsync();

                    var responseDtos = result.Select(currency => new CurrencyResponseDto
                    {
                        Id = currency.Id,
                        Name = currency.Name,
                        Symbol = currency.Symbol,
                        Code = currency.Code,
                        //ExchangeRate = currency.ExchangeRate,
                        //IsDefault = currency.IsDefault,
                        //IsActive = currency.IsActive,
                        ProductsCount = currency.Products?.Count ?? 0,
                        //CreatedAt = currency.CreatedAt,
                        //LastModifiedAt = currency.LastModifiedAt,
                        //IsDeleted = currency.IsDeleted
                    });

                    currencies = await Result<IEnumerable<CurrencyResponseDto>>.SuccessAsync(responseDtos, "Currencies retrieved successfully.", true);

                    if (currencies.Data.Count() > 0)
                    {
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024,
                        };
                        _memoryCache.Set(CacheKey, currencies, cacheExpiryOptions);

                        return await Result<IEnumerable<CurrencyResponseDto>>.SuccessAsync(currencies.Data, "Currencies retrieved successfully.", true);
                    }
                }

                return currencies ?? await Result<IEnumerable<CurrencyResponseDto>>.FaildAsync(false, "No currencies found.");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<CurrencyResponseDto>>.FaildAsync(false, $"Error retrieving currencies: {ex.Message}");
            }
        }
    }
} 