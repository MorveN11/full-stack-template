using Domain.Users;
using SharedKernel.Domain;

namespace Domain.UserProfiles;

public sealed class UserProfile : Entity
{
    public required Guid UserId { get; init; }
    public required Uri? PictureUrl { get; init; }

    public User User { get; init; } = null!;
}
