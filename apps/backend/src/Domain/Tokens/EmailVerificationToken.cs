using Domain.Users;

namespace Domain.Tokens;

public sealed class EmailVerificationToken
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required Guid UserId { get; init; }

    public required DateTime CreatedOnUtc { get; init; }

    public User User { get; init; } = null!;
}
