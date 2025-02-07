using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Joins;
using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register;

internal sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        Role? defaultRole = await context
            .Roles.AsNoTracking()
            .Where(r => r.Name == "User")
            .FirstOrDefaultAsync(cancellationToken);

        if (defaultRole == null)
        {
            return Result.Failure<Guid>(UserErrors.DefaultRoleNotFound);
        }

        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = passwordHasher.Hash(command.Password),
            UserRoles =
            [
                new UserRole { RoleId = defaultRole.Id, CreatedOnUtc = timeProvider.UtcNow },
            ],
            CreatedOnUtc = timeProvider.UtcNow,
        };

        user.Raise(new UserRegisteredDomainEvent(user.Id, user.Email));

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
