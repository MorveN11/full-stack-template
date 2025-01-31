using Application.Abstractions.Messaging;

namespace Application.Commands.Users.RevokeRefreshTokens;

public sealed record RevokeRefreshTokensCommand(Guid UserId) : ICommand;
