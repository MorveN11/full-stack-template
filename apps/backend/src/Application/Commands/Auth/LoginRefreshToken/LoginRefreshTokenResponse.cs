namespace Application.Commands.Auth.LoginRefreshToken;

public sealed record LoginRefreshTokenResponse
{
    public required string AccessToken { get; init; }

    public required string RefreshToken { get; init; }
};
