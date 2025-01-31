using Presentation.Extensions;
using Presentation.Infrastructure;

namespace Presentation;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();

        services.AddSignalR();

        services.AddCorsInternal(configuration);

        services.AddRedisCache(configuration);

        services.AddFluentEmailInternal(configuration, environment);

        return services;
    }
}
