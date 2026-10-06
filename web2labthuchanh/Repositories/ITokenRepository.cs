using Microsoft.AspNetCore.Identity;

namespace web2labthuchanh.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}