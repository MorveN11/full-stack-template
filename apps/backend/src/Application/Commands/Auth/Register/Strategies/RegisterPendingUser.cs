using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Entities.Auth.Users;
using Domain.Entities.Auth.Users.DomainEvents;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register.Strategies;

internal sealed class RegisterPendingUser(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider,
    User user
) : IRegisterUser
{
    public Guid RegisterUser(RegisterUserCommand command)
    {
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.PasswordHash = passwordHasher.Hash(command.Password);
        user.UpdatedOnUtc = timeProvider.UtcNow;

        user.Raise(new PendingUserRegisteredDomainEvent(user.Id, user.Email));

        context.Users.Update(user);

        return user.Id;
    }
}
