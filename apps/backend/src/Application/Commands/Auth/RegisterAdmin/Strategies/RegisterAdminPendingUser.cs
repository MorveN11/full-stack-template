using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Domain.Joins;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Time;

namespace Application.Commands.Auth.RegisterAdmin.Strategies;

internal sealed class RegisterAdminPendingUser(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider,
    User user
) : IRegisterAdminUser
{
    public async Task<Guid> RegisterUserAsync(
        RegisterAdminUserCommand command,
        List<Role> roles,
        CancellationToken cancellationToken = default
    )
    {
        await context
            .UserRoles.Where(ur => ur.UserId == user.Id)
            .ExecuteDeleteAsync(cancellationToken);

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.PasswordHash = passwordHasher.Hash(command.Password);
        user.UpdatedOnUtc = timeProvider.UtcNow;
        user.UserRoles = roles
            .Select(role => new UserRole { RoleId = role.Id, CreatedOnUtc = timeProvider.UtcNow })
            .ToList();
        user.EmailVerified = true;
        user.Status = UserStatus.Active;

        context.Users.Update(user);

        await context
            .OtpCodes.Where(o => o.UserId == user.Id && o.Type == OtpCodeType.Register)
            .ExecuteDeleteAsync(cancellationToken);

        return user.Id;
    }
}
