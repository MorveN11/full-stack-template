using Domain.Enums;
using Domain.Joins;
using Domain.Roles;
using Domain.UserProfiles;
using SharedKernel.Domain;

namespace Domain.Users;

public sealed class User : Entity
{
    public required string Email { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Pending;
    public UserProfile? Profile { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<UserRole> UserRoles { get; set; } = [];
}
