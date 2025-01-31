using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Utilities;
using Domain.Joins;
using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Users.Register;

internal sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider,
    ICacheStore cacheStore
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

        foreach (Guid roleId in command.Roles)
        {
            if (!await context.Roles.AnyAsync(r => r.Id == roleId, cancellationToken))
            {
                return Result.Failure<Guid>(RoleErrors.NotFound(roleId));
            }
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            PasswordHash = passwordHasher.Hash(command.Password),
            UserRoles = command
                .Roles.Select(r => new UserRole { RoleId = r, CreatedAt = timeProvider.UtcNow })
                .ToList(),
            CreatedAt = timeProvider.UtcNow,
        };

        user.Raise(new UserRegisteredDomainEvent(user.Id, user.Email));

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        await cacheStore.EvictByTagAsync(Tags.Users, cancellationToken);

        return user.Id;
    }
}
