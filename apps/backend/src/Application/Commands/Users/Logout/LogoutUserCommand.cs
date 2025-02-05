using Application.Abstractions.Messaging;

namespace Application.Commands.Users.Logout;

public sealed record LogoutUserCommand(Guid UserId) : ICommand;
