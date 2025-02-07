using Domain.Users;
using SharedKernel.Domain;

namespace Domain.Tokens;

public sealed class EmailVerificationToken : Entity
{
    public required Guid UserId { get; init; }
    public User User { get; init; } = null!;
}
