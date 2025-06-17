namespace Alwalid.Cms.Api.Features.Category.Dtos
{
    public class CategoryRequestDto
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
} 