using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Commands.Auth.RegisterAdmin.Strategies;
using Domain.Authorization;
using Domain.Enums;
using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.RegisterAdmin;

internal sealed class RegisterAdminUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider,
    IAuthorizationHandler authorizationHandler
) : ICommandHandler<RegisterAdminUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterAdminUserCommand command,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            new PermissionAccess(Resources.Users, CrudOperations.Create),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure<Guid>(authorizationResult.Error);
        }

        User? user = await context.Users.SingleOrDefaultAsync(
            u => u.Email == command.Email,
            cancellationToken
        );

        if (user != null && user.Status != UserStatus.Pending)
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        List<Role> roles = [];

        foreach (string role in command.Roles)
        {
            Role? roleEntity = await context.Roles.SingleOrDefaultAsync(
                r => r.Name == role,
                cancellationToken
            );

            if (roleEntity is null)
            {
                return Result.Failure<Guid>(RoleErrors.NotFound(role));
            }

            roles.Add(roleEntity);
        }

        IRegisterAdminUser registerUser =
            user != null
                ? new RegisterAdminPendingUser(context, passwordHasher, timeProvider, user)
                : new RegisterAdminNewUser(context, passwordHasher, timeProvider);

        Guid userId = await registerUser.RegisterUserAsync(command, roles, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return userId;
    }
}
