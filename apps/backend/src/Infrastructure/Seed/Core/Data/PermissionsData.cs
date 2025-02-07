using Domain.Permissions;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data;

internal sealed class PermissionsData(IDateTimeProvider timeProvider)
    : SeedEntity<Permission>(DbPriority.One, SeedEnvironment.Production)
{
    protected override IEnumerable<Permission> GetData()
    {
        return
        [
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
                Id = PermissionsId.RevokeTokensUsers,
                Name = "users:revoketokens",
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
                Id = PermissionsId.RevokeTokensSelfUser,
                Name = "users:revoketokens:self",
                CreatedOnUtc = timeProvider.UtcNow,
            },
        ];
    }
}
