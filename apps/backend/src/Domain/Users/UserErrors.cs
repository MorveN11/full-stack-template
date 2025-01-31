using SharedKernel.Errors;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Users.NotFound", $"The user with ID {userId} was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique"
    );

    public static readonly Error InvalidCredentials = Error.Problem(
        "Users.InvalidCredentials",
        "The email or password is incorrect"
    );

    public static readonly Error Unauthorized = Error.Problem(
        "Users.Unauthorized",
        "You are not authorized to perform this action."
    );

    public static readonly Error EmailNotVerified = Error.Problem(
        "Users.EmailNotVerified",
        "The email address is not verified"
    );
}
