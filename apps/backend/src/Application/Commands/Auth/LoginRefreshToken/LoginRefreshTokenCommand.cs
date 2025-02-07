using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.LoginRefreshToken;

public sealed record LoginRefreshTokenCommand(string RefreshToken)
    : ICommand<LoginRefreshTokenResponse>;
