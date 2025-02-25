using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Authorization;
using Domain.Entities.Auth.Users;
using Domain.Identifiers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Application.Commands.Auth.UpdatePassword;

internal sealed class UpdatePasswordCommandHandler(
    IApplicationDbContext context,
    IAuthorizationHandler authorizationHandler,
    IPasswordHasher passwordHasher,
    ICacheService cacheService
) : ICommandHandler<UpdatePasswordCommand>
{
    public async Task<Result> Handle(
        UpdatePasswordCommand command,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            command.UserId,
            new PermissionAccess(Resources.Users, CrudOperations.Update),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure(authorizationResult.Error);
        }

        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (!User.IsValidUser(user, command.Email, out Error? error))
        {
            return Result.Failure(error!);
        }

        user!.PasswordHash = passwordHasher.Hash(command.NewPassword);

        context.Users.Update(user);

        if (command.SignOutEverywhere)
        {
            await context
                .RefreshTokens.Where(rt => rt.UserId == user.Id && rt.Token != command.RefreshToken)
                .ExecuteDeleteAsync(cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Auth, cancellationToken);

        return Result.Success();
    }
}
