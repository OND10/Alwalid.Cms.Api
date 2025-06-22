using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Dtos;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetByCode
{
    public class GetByCodeQueryHandler : IQueryHandler<GetByCodeQuery, CurrencyResponseDto>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetByCodeQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<Result<CurrencyResponseDto>> Handle(GetByCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var currency = await _currencyRepository.GetByCodeAsync(query.Code);

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

                return await Result<CurrencyResponseDto>.SuccessAsync(responseDto, "Currency retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<CurrencyResponseDto>.FaildAsync(false, $"Error retrieving currency: {ex.Message}");
            }
        }
    }
}