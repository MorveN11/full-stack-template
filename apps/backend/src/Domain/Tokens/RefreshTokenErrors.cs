using SharedKernel.Errors;

namespace Domain.Tokens;

public static class RefreshTokenErrors
{
    public static readonly Error TokenExpired = Error.Unauthorized(
        "Tokens.TokenExpired",
        "The refresh token has expired"
    );
}
