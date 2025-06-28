namespace Alwalid.Cms.Api.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Comment {  get; set; }
        public string ArabicFullName { get; set; }
        public string EnglishFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl {  get; set; }
        public string Position {  get; set; }
    }
}
