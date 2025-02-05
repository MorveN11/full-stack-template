using System.IdentityModel.Tokens.Jwt;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Time;

namespace Infrastructure.Services;

internal sealed class CacheService(
    IOutputCacheStore cacheStore,
    IDistributedCache distributedCache,
    IDateTimeProvider timeProvider
) : ICacheService
{
    public async Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        await cacheStore.EvictByTagAsync(tag, cancellationToken);
    }

    public async Task BlacklistTokenAsync(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        JwtSecurityToken jwtToken = new(token);

        TimeSpan expiration = jwtToken.ValidTo - timeProvider.UtcNow;

        await distributedCache.SetStringAsync(
            $"blacklist:{token}",
            "revoked",
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration },
            cancellationToken
        );
    }

    public async Task<bool> IsTokenBlacklistedAsync(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        string? result = await distributedCache.GetStringAsync(
            $"blacklist:{token}",
            cancellationToken
        );

        return !string.IsNullOrEmpty(result);
    }
}
