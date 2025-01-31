using SharedKernel.Errors;

namespace Domain.Tokens;

public static class EmailVerificationTokenErrors
{
    public static readonly Error TokenExpired = Error.Problem(
        "Tokens.TokenExpired",
        "The email verification token has expired"
    );

    public static readonly Error UserAlreadyVerified = Error.Problem(
        "Tokens.UserAlreadyVerified",
        "The user has already been verified"
    );
}
