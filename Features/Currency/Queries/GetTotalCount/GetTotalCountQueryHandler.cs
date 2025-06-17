using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Currency;

namespace Alwalid.Cms.Api.Features.Currency.Queries.GetTotalCount
{
    public class GetTotalCountQueryHandler : IQueryHandler<GetTotalCountQuery, int>
    {
        private readonly ICurrencyRepository _currencyRepository;

        public GetTotalCountQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<Result<int>> Handle(GetTotalCountQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var count = await _currencyRepository.GetTotalCountAsync();

                return await Result<int>.SuccessAsync(count, "Total currency count retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<int>.FaildAsync(false, $"Error retrieving total currency count: {ex.Message}");
            }
        }
    }
} 