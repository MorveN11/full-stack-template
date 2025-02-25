using Domain.Entities.Auth.Users;
using SharedKernel.Domain;

namespace Domain.Entities.Auth.UserProfiles;

public sealed class UserProfile : Entity
{
    public required Guid UserId { get; init; }
    public required Uri? PictureUrl { get; set; }

    public User User { get; init; } = null!;
}
