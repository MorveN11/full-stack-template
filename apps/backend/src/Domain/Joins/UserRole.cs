using Domain.Roles;
using Domain.Users;
using SharedKernel.Domain;

namespace Domain.Joins;

public sealed class UserRole : Register
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public User User { get; init; } = null!;
    public Role Role { get; init; } = null!;
}
