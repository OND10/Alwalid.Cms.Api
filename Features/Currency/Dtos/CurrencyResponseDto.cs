namespace Alwalid.Cms.Api.Features.Currency.Dtos
{
    public class CurrencyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public int ProductsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
} 