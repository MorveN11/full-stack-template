using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Joins;
using Domain.Roles;
using Domain.UserProfiles;
using Domain.Users;
using Domain.Users.DomainEvents;
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
