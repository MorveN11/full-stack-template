using Presentation.Extensions;
using Presentation.Infrastructure;

namespace Presentation;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddEndpointsApiExplorer();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        services.AddSignalR();

        services.AddCorsInternal(configuration);

        services.AddRateLimiterInternal(configuration);

        return services;
    }
}
