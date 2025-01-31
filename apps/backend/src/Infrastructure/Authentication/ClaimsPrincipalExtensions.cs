using System.Security.Claims;

namespace Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("User id is unavailable");
    }

    public static string GetEmail(this ClaimsPrincipal? principal)
    {
        string? email = principal?.FindFirstValue(ClaimTypes.Email);

        return email ?? throw new ApplicationException("Email is unavailable");
    }

    public static bool GetEmailVerified(this ClaimsPrincipal? principal)
    {
        string? emailVerified = principal?.FindFirstValue("email_verified");

        return bool.TryParse(emailVerified, out bool parsedEmailVerified)
            ? parsedEmailVerified
            : throw new ApplicationException("Email verification status is unavailable");
    }
}
