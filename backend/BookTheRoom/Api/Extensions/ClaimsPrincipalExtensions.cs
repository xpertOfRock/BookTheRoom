using System.Security.Claims;

namespace Api.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        internal static string? GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        internal static string? GetUsername(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.Name);
            return claim?.Value;
        }

        internal static string? GetFullName(this ClaimsPrincipal user)
        {
            var name = user.FindFirst(ClaimTypes.GivenName)?.Value;
            var surname = user.FindFirst(ClaimTypes.Surname)?.Value;
            if (name == null && surname == null) return null;
            return $"{name} {surname}".Trim();
        }

        internal static string? GetPhoneNumber(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.HomePhone);
            return claim?.Value;
        }

        internal static string? GetEmail(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
