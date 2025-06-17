namespace Alwalid.Cms.Api.Features.ProductImage.Dtos
{
    public class ProductImageResponseDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
} 