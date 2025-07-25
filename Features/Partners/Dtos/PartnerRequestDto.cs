namespace Alwalid.Cms.Api.Features.Partners.Dtos
{
    public class PartnerRequestDto
    {
        public string ArabicName { get; set; }
        public string ArabicDescription { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public IFormFile file {  get; set; }
    }
}
