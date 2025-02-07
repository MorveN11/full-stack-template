using System.IdentityModel.Tokens.Jwt;
using Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using SharedKernel.Time;

namespace Infrastructure.Services;

internal sealed class CacheService(
    HybridCache hybridCache,
    IDistributedCache distributedCache,
    IDateTimeProvider timeProvider
) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        IEnumerable<string> tags,
        CancellationToken cancellationToken = default
    )
    {
        T result = await hybridCache.GetOrCreateAsync(
            key,
            factory,
            tags: tags,
            cancellationToken: cancellationToken
        );

        return result;
    }

    public async Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        await hybridCache.RemoveByTagAsync(tag, cancellationToken);
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
