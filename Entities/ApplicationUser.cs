using Microsoft.AspNetCore.Identity;

namespace Alwalid.Cms.Api.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string UserName {  get; set; } = string.Empty;

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
