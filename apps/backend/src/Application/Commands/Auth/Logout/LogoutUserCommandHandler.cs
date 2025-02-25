using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Authorization;
using Domain.Entities.Auth.RefreshTokens;
using Domain.Identifiers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Auth.Logout;

internal sealed class LogoutUserCommandHandler(
    IApplicationDbContext context,
    IAuthorizationHandler authorizationHandler,
    IUserContext userContext,
    ICacheService cacheService
) : ICommandHandler<LogoutUserCommand>
{
    public async Task<Result> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            command.UserId,
            new PermissionAccess(Resources.Users, AuthOperations.Logout),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure(authorizationResult.Error);
        }

        RefreshToken? refreshToken = await context.RefreshTokens.SingleOrDefaultAsync(
            r => r.UserId == command.UserId && r.Token == command.RefreshToken,
            cancellationToken
        );

        if (refreshToken is null)
        {
            return Result.Failure(RefreshTokenErrors.SessionNotFound);
        }

        context.RefreshTokens.Remove(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        string jwt = userContext.Jwt;

        await cacheService.BlacklistTokenAsync(jwt, cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Users, cancellationToken);

        return Result.Success();
    }
}
