namespace Alwalid.Cms.Api.Features.Branch.Dtos
{
    public class BranchRequestDto
    {
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
} 