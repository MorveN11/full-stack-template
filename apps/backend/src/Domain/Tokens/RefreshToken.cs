using Domain.Users;
using SharedKernel.Domain;

namespace Domain.Tokens;

public sealed class RefreshToken : Entity
{
    public required string Token { get; set; }
    public required Guid UserId { get; init; }
    public required DateTime ExpiredOnUtc { get; set; }
    public User User { get; init; } = null!;
}
