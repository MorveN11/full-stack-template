namespace Application.Commands.Auth.ResetPassword;

public sealed record ResetPasswordResponse
{
    public required Guid UserId { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required Uri? PictureUrl { get; init; }

    public required HashSet<string> Roles { get; init; }

    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }
}
