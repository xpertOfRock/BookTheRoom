using BookTheRoom.Infrastructure.Identity;

namespace BookTheRoom.Web.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, string role);
    }
}
