using Domain.Joins;
using Domain.Roles;
using SharedKernel.Domain;

namespace Domain.Permissions;

public sealed class Permission : Entity
{
    public string Name { get; init; }
    public List<Role> Roles { get; init; } = [];
    public List<RolePermission> RolePermissions { get; init; } = [];
}
