using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal static class RequestExtensions
{
    public static string GetJwtToken(this HttpRequest request)
    {
        string? bearerToken = request.Headers.Authorization;

        if (string.IsNullOrWhiteSpace(bearerToken))
        {
            throw new ApplicationException("JWT token is unavailable");
        }

        return bearerToken.StartsWith("Bearer ", StringComparison.Ordinal)
            ? bearerToken["Bearer ".Length..]
            : throw new ApplicationException("JWT token is unavailable");
    }
}
