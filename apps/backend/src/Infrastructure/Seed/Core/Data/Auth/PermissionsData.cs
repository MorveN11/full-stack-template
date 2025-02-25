using Domain.Entities.Auth.Permissions;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids.Auth;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data.Auth;

internal sealed class PermissionsData(IDateTimeProvider timeProvider)
    : SeedEntity<Permission>(DbPriority.One, SeedEnvironment.Production)
{
    protected override IEnumerable<Permission> GetData()
    {
        return
        [
            new Permission
            {
                Id = PermissionsId.CreateUsers,
                Name = "users:create",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.ReadUsers,
                Name = "users:read",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.UpdateUsers,
                Name = "users:update",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.DeleteUsers,
                Name = "users:delete",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.LogoutUsers,
                Name = "users:logout",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.FinishSessions,
                Name = "users:finish_sessions",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.GetSessions,
                Name = "users:get_sessions",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.ReadSelfUser,
                Name = "users:read:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.UpdateSelfUser,
                Name = "users:update:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.DeleteSelfUser,
                Name = "users:delete:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.LogoutSelfUser,
                Name = "users:logout:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.FinishSelfSessions,
                Name = "users:finish_sessions:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Permission
            {
                Id = PermissionsId.GetSelfSessions,
                Name = "users:get_sessions:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
        ];
    }
}
