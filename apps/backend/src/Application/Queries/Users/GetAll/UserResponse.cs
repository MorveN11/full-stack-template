namespace Application.Queries.Users.GetAll;

public sealed record UserResponse
{
    public required Guid Id { get; init; }

    public required string Email { get; init; }
}
