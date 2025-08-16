namespace Alwalid.Cms.Api.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string Email { get; set; } = default!;
        public string MarketName { get; set; } = default!;
        public bool ReceiveCategoryNotifications { get; set; } = true;
    }
}
