namespace Alwalid.Cms.Api.Features.Department.Dtos
{
    public class DepartmentRequestDto
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public int BranchId { get; set; }
    }
} 