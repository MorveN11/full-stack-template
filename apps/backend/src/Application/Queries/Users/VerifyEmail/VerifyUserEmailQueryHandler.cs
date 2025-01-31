using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Utilities;
using Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Queries.Users.VerifyEmail;

internal sealed class VerifyUserEmailQueryHandler(
    IApplicationDbContext context,
    ICacheStore cacheStore
) : IQueryHandler<VerifyUserEmailQuery, string>
{
    public async Task<Result<string>> Handle(
        VerifyUserEmailQuery query,
        CancellationToken cancellationToken
    )
    {
        EmailVerificationToken? token = await context
            .EmailVerificationTokens.Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == query.TokenId, cancellationToken);

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

        await cacheStore.EvictByTagAsync(Tags.Users, cancellationToken);

        return Result.Success("Email verified successfully");
    }
}
