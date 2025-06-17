namespace Alwalid.Cms.Api.Features.Department.Dtos
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchCity { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public int ProductsCount { get; set; }
    }
} 