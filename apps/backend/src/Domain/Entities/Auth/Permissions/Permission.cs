using Domain.Entities.Auth.Roles;
using Domain.Joins;
using SharedKernel.Domain;

namespace Domain.Entities.Auth.Permissions;

public sealed class Permission : Entity
{
    public string Name { get; init; }
    public List<Role> Roles { get; init; } = [];
    public List<RolePermission> RolePermissions { get; init; } = [];
}
