using Domain.Entities.Auth.Roles;
using Domain.Joins;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids.Auth;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data.Auth;

internal sealed class RolesData(IDateTimeProvider timeProvider)
    : SeedEntity<Role>(DbPriority.Two, SeedEnvironment.Production)
{
    protected override IEnumerable<Role> GetData()
    {
        return
        [
            new Role
            {
                Id = Role.UserId,
                Name = Role.User,
                RolePermissions =
                [
                    new RolePermission
                    {
                        PermissionId = PermissionsId.ReadSelfUser,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.UpdateSelfUser,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.DeleteSelfUser,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.LogoutSelfUser,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.FinishSelfSessions,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.GetSelfSessions,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                ],
                CreatedOnUtc = timeProvider.UtcNow,
            },
            new Role
            {
                Id = Role.AdminId,
                Name = Role.Admin,
                RolePermissions =
                [
                    new RolePermission
                    {
                        PermissionId = PermissionsId.CreateUsers,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.ReadUsers,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.UpdateUsers,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.DeleteUsers,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.LogoutUsers,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.FinishSessions,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                    new RolePermission
                    {
                        PermissionId = PermissionsId.GetSessions,
                        CreatedOnUtc = timeProvider.UtcNow,
                    },
                ],
                CreatedOnUtc = timeProvider.UtcNow,
            },
        ];
    }
}
