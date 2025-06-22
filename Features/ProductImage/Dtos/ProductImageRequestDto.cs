namespace Alwalid.Cms.Api.Features.ProductImage.Dtos
{
    public class ProductImageRequestDto
    {
        public int ProductId { get; set; }
        public IFormFile image { get; set; }
    }
} 