using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Identifiers;
using Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Auth.VerifyEmail;

internal sealed class VerifyUserEmailCommandHandler(
    IApplicationDbContext context,
    ICacheService cacheService
) : ICommandHandler<VerifyUserEmailCommand>
{
    public async Task<Result> Handle(
        VerifyUserEmailCommand command,
        CancellationToken cancellationToken
    )
    {
        EmailVerificationToken? token = await context
            .EmailVerificationTokens.Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == command.TokenId, cancellationToken);

        if (token is null)
        {
            return Result.Failure<string>(EmailVerificationTokenErrors.TokenExpired);
        }

        if (token.User.EmailVerified)
        {
            return Result.Failure<string>(EmailVerificationTokenErrors.UserAlreadyVerified);
        }

        token.User.EmailVerified = true;

        context.EmailVerificationTokens.Remove(token);

        await context.SaveChangesAsync(cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Users, cancellationToken);

        return Result.Success();
    }
}
