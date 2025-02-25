using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.UserProfiles;
using Domain.Entities.Auth.Users;
using Domain.Entities.Auth.Users.DomainEvents;
using Domain.Joins;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register.Strategies;

internal sealed class RegisterNewUser(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider
) : IRegisterUser
{
    public Guid RegisterUser(RegisterUserCommand command)
    {
        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = passwordHasher.Hash(command.Password),
            UserRoles = [new UserRole { RoleId = Role.UserId, CreatedOnUtc = timeProvider.UtcNow }],
            CreatedOnUtc = timeProvider.UtcNow,
        };

        user.Raise(new NewUserRegisteredDomainEvent(user.Id, user.Email));

        var userProfile = new UserProfile
        {
            UserId = user.Id,
            PictureUrl = null,
            CreatedOnUtc = timeProvider.UtcNow,
        };

        context.Users.Add(user);
        context.UserProfiles.Add(userProfile);

        return user.Id;
    }
}
