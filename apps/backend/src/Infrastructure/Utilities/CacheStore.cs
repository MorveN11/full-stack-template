using Application.Abstractions.Utilities;
using Microsoft.AspNetCore.OutputCaching;

namespace Infrastructure.Utilities;

internal sealed class CacheStore(IOutputCacheStore cacheStore) : ICacheStore
{
    public async Task EvictByTagAsync(string tag, CancellationToken cancellationToken)
    {
        await cacheStore.EvictByTagAsync(tag, cancellationToken);
    }
}
