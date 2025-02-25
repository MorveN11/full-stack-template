namespace Application.Queries.Auth.GetSessionsById;

public sealed record SessionResponse
{
    public required Guid Id { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime LastAccessedOnUtc { get; init; }
    public required string? IpAddress { get; init; }
    public required string? UserAgent { get; init; }
}
