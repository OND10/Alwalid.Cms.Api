using Microsoft.AspNetCore.Identity;

namespace Alwalid.Cms.Api.Entities
{
    public class ApplicationUser : IdentityUser
    {
        

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
