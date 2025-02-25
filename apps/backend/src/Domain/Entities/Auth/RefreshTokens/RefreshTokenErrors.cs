using SharedKernel.Errors;

namespace Domain.Entities.Auth.RefreshTokens;

public static class RefreshTokenErrors
{
    public static readonly Error TokenExpired = Error.Unauthorized(
        "Tokens.TokenExpired",
        "The refresh token has expired"
    );

    public static readonly Error SessionNotFound = Error.NotFound(
        "Users.SessionNotFound",
        "The session was not found"
    );
}
