using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Users.RevokeRefreshTokens;

internal sealed class RevokeRefreshTokensCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext
) : ICommandHandler<RevokeRefreshTokensCommand>
{
    public async Task<Result> Handle(
        RevokeRefreshTokensCommand command,
        CancellationToken cancellationToken
    )
    {
        if (command.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized);
        }

        await context
            .RefreshTokens.Where(r => r.UserId == command.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }
}
