using Application.Abstractions.Authentication;
using Domain.Joins;
using Domain.Roles;
using Domain.Users;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data;

internal sealed class UsersData(IPasswordHasher passwordHasher, IDateTimeProvider timeProvider)
    : SeedEntity<User>(DbPriority.Three)
{
    protected override IEnumerable<User> GetData()
    {
        return
        [
            new User
            {
                Id = UsersId.User,
                Email = "user@user.com",
                FirstName = "user",
                LastName = "user",
                PasswordHash = passwordHasher.Hash("user"),
                EmailVerified = true,
                UserRoles =
                [
                    new UserRole { RoleId = Role.UserId, CreatedOnUtc = timeProvider.UtcNow },
                ],
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new User
            {
                Id = UsersId.Admin,
                Email = "admin@admin.com",
                FirstName = "admin",
                LastName = "admin",
                PasswordHash = passwordHasher.Hash("admin"),
                EmailVerified = true,
                UserRoles =
                [
                    new UserRole { RoleId = Role.AdminId, CreatedOnUtc = timeProvider.UtcNow },
                    new UserRole { RoleId = Role.UserId, CreatedOnUtc = timeProvider.UtcNow },
                ],
                CreatedOnUtc = timeProvider.UtcNow,
            },
        ];
    }
}
