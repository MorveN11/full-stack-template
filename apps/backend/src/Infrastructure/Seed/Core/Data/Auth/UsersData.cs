using Application.Abstractions.Authentication;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Domain.Joins;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids.Auth;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data.Auth;

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
                Status = UserStatus.Active,
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
                Status = UserStatus.Active,
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
