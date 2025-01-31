using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Users.LoginRefreshToken;

internal sealed class LoginRefreshTokenCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider,
    IDateTimeProvider timeProvider
) : ICommandHandler<LoginRefreshTokenCommand, LoginRefreshTokenResponse>
{
    public async Task<Result<LoginRefreshTokenResponse>> Handle(
        LoginRefreshTokenCommand command,
        CancellationToken cancellationToken
    )
    {
        RefreshToken? refreshToken = await context
            .RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == command.RefreshToken, cancellationToken);

        if (refreshToken is null || refreshToken.ExpiredOnUtc < timeProvider.UtcNow)
        {
            return Result.Failure<LoginRefreshTokenResponse>(RefreshTokenErrors.TokenExpired);
        }

        string accessToken = tokenProvider.Create(refreshToken.User);

        refreshToken.Token = tokenProvider.GenerateRefreshToken();

        refreshToken.ExpiredOnUtc = timeProvider.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);

        var response = new LoginRefreshTokenResponse(accessToken, refreshToken.Token);

        return response;
    }
}
