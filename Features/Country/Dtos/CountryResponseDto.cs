namespace Alwalid.Cms.Api.Features.Country.Dtos
{
    public class CountryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int BranchesCount { get; set; }
    }
} 