using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Commands.Auth.Register.Strategies;
using Domain.Enums;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register;

internal sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IDateTimeProvider timeProvider
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context.Users.SingleOrDefaultAsync(
            u => u.Email == command.Email,
            cancellationToken
        );

        if (user != null && user.Status != UserStatus.Pending)
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        IRegisterUser registerUser =
            user != null
                ? new RegisterPendingUser(context, passwordHasher, timeProvider, user)
                : new RegisterNewUser(context, passwordHasher, timeProvider);

        Guid userId = registerUser.RegisterUser(command);

        await context.SaveChangesAsync(cancellationToken);

        return userId;
    }
}
