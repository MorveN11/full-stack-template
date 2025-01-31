using SharedKernel.Domain;

namespace Domain.Joins;

public sealed class UserRole : Register
{
    public Guid UserId { get; init; }

    public Guid RoleId { get; init; }
}
