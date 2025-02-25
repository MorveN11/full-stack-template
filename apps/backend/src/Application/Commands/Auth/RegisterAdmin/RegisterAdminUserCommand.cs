using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.RegisterAdmin;

public sealed record RegisterAdminUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    List<string> Roles
) : ICommand<Guid>;
