using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.Logout;

public sealed record LogoutUserCommand(Guid UserId, string RefreshToken) : ICommand;
