using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
