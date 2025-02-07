using Domain.Joins;
using Domain.Permissions;
using Domain.Users;
using SharedKernel.Domain;

namespace Domain.Roles;

public sealed class Role : Entity
{
    public static readonly Guid UserId = Guid.Parse("0194d7d8-259d-7dc7-965f-765b2efa55c1");
    public const string User = "User";

    public static readonly Guid AdminId = Guid.Parse("0194d7d8-3879-7dc3-a392-6345c9ac387d");
    public const string Admin = "Admin";

    public required string Name { get; init; }
    public List<User> Users { get; init; } = [];
    public List<UserRole> UserRoles { get; init; } = [];
    public List<Permission> Permissions { get; init; } = [];
    public List<RolePermission> RolePermissions { get; init; } = [];
}
