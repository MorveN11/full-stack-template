using Domain.Roles;

namespace Application.Commands.Auth.RegisterAdmin.Strategies;

internal interface IRegisterAdminUser
{
    Task<Guid> RegisterUserAsync(
        RegisterAdminUserCommand command,
        List<Role> roles,
        CancellationToken cancellationToken = default
    );
}
