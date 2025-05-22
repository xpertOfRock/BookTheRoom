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

        internal static string GetFullName(this ClaimsPrincipal user)
        {
            return $"{user.FindFirst(ClaimTypes.GivenName)!.Value} {user.FindFirst(ClaimTypes.Surname)!.Value}";
        }

        internal static string GetPhoneNumber(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.HomePhone)!.Value;
        }
        internal static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)!.Value;
        }
    }
}
