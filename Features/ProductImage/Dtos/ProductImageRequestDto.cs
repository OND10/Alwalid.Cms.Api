namespace Alwalid.Cms.Api.Features.ProductImage.Dtos
{
    public class ProductImageRequestDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public IFormFile image { get; set; }
    }
} 