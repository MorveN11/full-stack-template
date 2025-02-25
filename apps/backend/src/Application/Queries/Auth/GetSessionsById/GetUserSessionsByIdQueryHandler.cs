using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Authorization;
using Domain.Identifiers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Queries.Auth.GetSessionsById;

internal sealed class GetUserSessionsByIdQueryHandler(
    IApplicationDbContext context,
    IDateTimeProvider timeProvider,
    IAuthorizationHandler authorizationHandler,
    ICacheService cacheService
) : IQueryHandler<GetUserSessionsByIdQuery, List<SessionResponse>>
{
    private async Task<Result<List<SessionResponse>>> GetUserSessionsAsync(
        GetUserSessionsByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        List<SessionResponse> sessions = await context
            .RefreshTokens.AsNoTracking()
            .Where(r => r.UserId == query.UserId && r.ExpiredOnUtc > timeProvider.UtcNow)
            .Select(r => new SessionResponse
            {
                Id = r.Id,
                RefreshToken = r.Token,
                LastAccessedOnUtc = r.UpdatedOnUtc ?? r.CreatedOnUtc,
                IpAddress = r.IpAddress,
                UserAgent = r.UserAgent,
            })
            .ToListAsync(cancellationToken);

        return sessions;
    }

    public async Task<Result<List<SessionResponse>>> Handle(
        GetUserSessionsByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            query.UserId,
            new PermissionAccess(Resources.Users, AuthOperations.GetSessions),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure<List<SessionResponse>>(authorizationResult.Error);
        }

        return await cacheService.GetOrCreateAsync(
            $"users:sessions:{query.UserId}",
            async ct => await GetUserSessionsAsync(query, ct),
            [Tags.Auth],
            cancellationToken
        );
    }
}
