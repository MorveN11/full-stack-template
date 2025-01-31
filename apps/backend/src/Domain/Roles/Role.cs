using Domain.Joins;
using Domain.Users;
using SharedKernel.Domain;

namespace Domain.Roles;

public sealed class Role : Entity
{
    public required string Name { get; init; }

    public List<User> Users { get; init; } = [];

    public List<UserRole> UserRoles { get; init; } = [];
}
