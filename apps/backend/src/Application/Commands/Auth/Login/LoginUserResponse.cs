namespace Application.Commands.Auth.Login;

public sealed record LoginUserResponse
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }
}
