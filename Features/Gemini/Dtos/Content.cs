namespace Alwalid.Cms.Api.Features.Gemini.Dtos
{
    public class Content
    {
        public string role { get; set; } = "user";
        public List<Part> parts { get; set; }
    }
}
