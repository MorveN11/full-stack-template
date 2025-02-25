using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.UserProfiles;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Domain.Joins;
using SharedKernel.Time;

namespace Application.Commands.Auth.RegisterAdmin.Strategies;

internal sealed class RegisterAdminNewUser(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider
) : IRegisterAdminUser
{
    public Task<Guid> RegisterUserAsync(
        RegisterAdminUserCommand command,
        List<Role> roles,
        CancellationToken cancellationToken = default
    )
    {
        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = passwordHasher.Hash(command.Password),
            CreatedOnUtc = timeProvider.UtcNow,
            Status = UserStatus.Active,
            EmailVerified = true,
            UserRoles = roles
                .Select(role => new UserRole
                {
                    RoleId = role.Id,
                    CreatedOnUtc = timeProvider.UtcNow,
                })
                .ToList(),
        };

        var userProfile = new UserProfile
        {
            UserId = user.Id,
            PictureUrl = null,
            CreatedOnUtc = timeProvider.UtcNow,
        };

        context.Users.Add(user);
        context.UserProfiles.Add(userProfile);

        return Task.FromResult(user.Id);
    }
}
