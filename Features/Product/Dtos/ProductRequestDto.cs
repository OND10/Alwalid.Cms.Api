namespace Alwalid.Cms.Api.Features.Product.Dtos
{
    public class ProductRequestDto
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public string EnglishDescription { get; set; } = string.Empty;
        public string ArabicDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int? CurrencyId { get; set; }
    }
} 