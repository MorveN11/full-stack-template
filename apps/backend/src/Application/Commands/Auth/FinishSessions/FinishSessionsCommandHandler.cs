using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Authorization;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Auth.FinishSessions;

internal sealed class FinishSessionsCommandHandler(
    IApplicationDbContext context,
    IAuthorizationHandler authorizationHandler,
    IUserContext userContext,
    ICacheService cacheService
) : ICommandHandler<FinishSessionsCommand>
{
    public async Task<Result> Handle(
        FinishSessionsCommand command,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            command.UserId,
            new PermissionAccess(Resources.Users, AuthOperations.FinishSessions),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure(authorizationResult.Error);
        }

        await context
            .RefreshTokens.Where(r => r.UserId == command.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        string jwt = userContext.Jwt;

        await cacheService.BlacklistTokenAsync(jwt, cancellationToken);

        return Result.Success();
    }
}
