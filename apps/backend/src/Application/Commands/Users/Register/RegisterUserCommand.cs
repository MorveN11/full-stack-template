using Application.Abstractions.Messaging;

namespace Application.Commands.Users.Register;

public sealed record RegisterUserCommand(string Email, string Password, List<Guid> Roles)
    : ICommand<Guid>;
