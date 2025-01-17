using System.Security.Claims;

namespace Api.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        internal static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
        internal static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)!.Value;
        }
    }
}
