using Domain.Permissions;
using Domain.Roles;
using SharedKernel.Domain;

namespace Domain.Joins;

public sealed class RolePermission : Register
{
    public Guid RoleId { get; init; }
    public Guid PermissionId { get; init; }
    public Role Role { get; init; } = null!;
    public Permission Permission { get; init; } = null!;
}
