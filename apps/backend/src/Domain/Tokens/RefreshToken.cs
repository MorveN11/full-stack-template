using Domain.Users;

namespace Domain.Tokens;

public sealed class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Token { get; set; }

    public required Guid UserId { get; init; }

    public required DateTime ExpiredOnUtc { get; set; }

    public User User { get; init; } = null!;
}
