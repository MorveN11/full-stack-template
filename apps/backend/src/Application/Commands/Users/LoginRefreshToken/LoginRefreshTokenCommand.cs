using Application.Abstractions.Messaging;

namespace Application.Commands.Users.LoginRefreshToken;

public sealed record LoginRefreshTokenCommand(string RefreshToken)
    : ICommand<LoginRefreshTokenResponse>;
