namespace Alwalid.Cms.Api.Features.Services.Dtos
{
    public class ServiceRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile file { get; set; }
    }
}
