using Domain.Users;
using SharedKernel.Domain;

namespace Domain.RefreshTokens;

public sealed class RefreshToken : Entity
{
    public required string Token { get; set; }
    public required Guid UserId { get; init; }
    public required DateTime ExpiredOnUtc { get; set; }
    public required string? IpAddress { get; init; }
    public required string? UserAgent { get; init; }
    public User User { get; init; } = null!;
}
