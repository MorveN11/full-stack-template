using Domain.Joins;
using Domain.Roles;
using SharedKernel.Domain;

namespace Domain.Users;

public sealed class User : Entity
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PasswordHash { get; init; }
    public bool EmailVerified { get; set; }
    public List<Role> Roles { get; init; } = [];
    public List<UserRole> UserRoles { get; init; } = [];
}
