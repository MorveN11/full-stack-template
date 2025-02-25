using SharedKernel.Errors;

namespace Domain.RefreshTokens;

public static class RefreshTokenErrors
{
    public static readonly Error TokenExpired = Error.Unauthorized(
        "Tokens.TokenExpired",
        "The refresh token has expired"
    );
}
