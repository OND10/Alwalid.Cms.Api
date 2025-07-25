namespace Alwalid.Cms.Api.Features.Product.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public string EnglishDescription { get; set; } = string.Empty;
        public string ArabicDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? CurrencyId { get; set; }
        public string? CurrencySymbol { get; set; }
        public int ImagesCount { get; set; }
        public int StatisticsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> ImageUrls = new List<string>();  
    }
} 