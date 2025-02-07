using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Presentation.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerGenWithAuth(
        this IServiceCollection services,
        string title
    )
    {
        return services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });

            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            OpenApiSecurityScheme securityScheme = new()
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            OpenApiSecurityRequirement securityRequirement = new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        },
                    },
                    []
                },
            };

            o.AddSecurityRequirement(securityRequirement);
        });
    }

    public static IServiceCollection AddCorsInternal(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .WithOrigins(configuration["AllowedOrigins"]!.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            )
        );

        return services;
    }

    public static IServiceCollection AddRateLimiterInternal(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                RateLimitPartition.GetFixedWindowLimiter(
                    configuration["RateLimiter:Key"]!,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = configuration.GetValue<int>("RateLimiter:PermitLimit"),
                        Window = TimeSpan.FromSeconds(
                            configuration.GetValue<int>("RateLimiter:WindowInSeconds")
                        ),
                    }
                )
            );
        });
    }
}
