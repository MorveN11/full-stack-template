using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Users.Logout;

internal sealed class LogoutUserCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    ICacheService cacheService
) : ICommandHandler<LogoutUserCommand>
{
    public async Task<Result> Handle(LogoutUserCommand command, CancellationToken cancellationToken)
    {
        if (command.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Forbidden);
        }

        await context
            .RefreshTokens.Where(r => r.UserId == command.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        string jwt = userContext.Jwt;

        await cacheService.BlacklistTokenAsync(jwt, cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Users, cancellationToken);

        return Result.Success();
    }
}
