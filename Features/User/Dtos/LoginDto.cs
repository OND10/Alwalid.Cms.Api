using System.ComponentModel.DataAnnotations;

namespace Alwalid.Cms.Api.Features.User.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
