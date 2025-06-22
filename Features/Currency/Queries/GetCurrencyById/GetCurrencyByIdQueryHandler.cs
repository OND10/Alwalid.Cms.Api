using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById
{
    public class GetCurrencyByIdQueryHandler : IQueryHandler<GetCurrencyByIdQuery, CurrencyResponseDto>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetCurrencyByIdQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<Result<CurrencyResponseDto>> Handle(GetCurrencyByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var currency = await _currencyRepository.GetByIdAsync(query.Id);

                if (currency == null)
                {
                    return await Result<CurrencyResponseDto>.FaildAsync(false, "Currency not found.");
                }

                var responseDto = new CurrencyResponseDto
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
                };

                return await Result<CurrencyResponseDto>.SuccessAsync(responseDto, "Currency retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<CurrencyResponseDto>.FaildAsync(false, $"Error retrieving currency: {ex.Message}");
            }
        }
    }
}