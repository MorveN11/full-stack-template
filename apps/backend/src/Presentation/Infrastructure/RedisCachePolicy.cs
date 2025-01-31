using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Presentation.Infrastructure;

internal sealed class RedisCachePolicy : IOutputCachePolicy
{
    ValueTask IOutputCachePolicy.CacheRequestAsync(
        OutputCacheContext context,
        CancellationToken cancellation
    )
    {
        bool flag = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = flag;
        context.AllowCacheStorage = flag;
        context.AllowLocking = true;
        context.CacheVaryByRules.QueryKeys = (StringValues)"*";
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeFromCacheAsync(
        OutputCacheContext context,
        CancellationToken cancellation
    )
    {
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeResponseAsync(
        OutputCacheContext context,
        CancellationToken cancellation
    )
    {
        HttpResponse response = context.HttpContext.Response;

        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        if (response.StatusCode == 200)
        {
            return ValueTask.CompletedTask;
        }

        context.AllowCacheStorage = false;

        return ValueTask.CompletedTask;
    }

    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        HttpRequest request = context.HttpContext.Request;

        return HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method);
    }
}
