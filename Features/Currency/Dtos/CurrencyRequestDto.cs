namespace Alwalid.Cms.Api.Features.Currency.Dtos
{
    public class CurrencyRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 