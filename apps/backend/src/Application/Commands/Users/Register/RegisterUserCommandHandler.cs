using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
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
    ICacheService cacheService
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

        foreach (string roleName in command.Roles)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == roleName, cancellationToken))
            {
                return Result.Failure<Guid>(RoleErrors.NotFound(roleName));
            }
        }

        List<Guid> roleIds = await context
            .Roles.AsNoTracking()
            .Where(r => command.Roles.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            PasswordHash = passwordHasher.Hash(command.Password),
            UserRoles = roleIds
                .Select(r => new UserRole { RoleId = r, CreatedAt = timeProvider.UtcNow })
                .ToList(),
            CreatedAt = timeProvider.UtcNow,
        };

        user.Raise(new UserRegisteredDomainEvent(user.Id, user.Email));

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Users, cancellationToken);

        return user.Id;
    }
}
