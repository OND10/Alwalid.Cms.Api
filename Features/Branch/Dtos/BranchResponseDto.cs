namespace Alwalid.Cms.Api.Features.Branch.Dtos
{
    public class BranchResponseDto
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int DepartmentsCount { get; set; }
    }
} 