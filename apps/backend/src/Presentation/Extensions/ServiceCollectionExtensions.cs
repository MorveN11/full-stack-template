using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Presentation.Infrastructure;

namespace Presentation.Extensions;

internal static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOutputCache(options =>
            options.AddBasePolicy(
                builder => builder.AddPolicy<RedisCachePolicy>().SetCacheKeyPrefix("custom-"),
                true
            )
        );

        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");

            options.InstanceName = configuration["Redis:InstanceName"];
        });

        return services;
    }

    public static IServiceCollection AddFluentEmailInternal(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        FluentEmailServicesBuilder builder = services.AddFluentEmail(
            configuration["Email:SenderEmail"],
            configuration["Email:Sender"]
        );

        if (environment.IsDevelopment() || environment.IsStaging())
        {
            builder.AddSmtpSender(
                configuration["Email:Host"],
                configuration.GetValue<int>("Email:Port")
            );
        }
        else
        {
            builder.AddSmtpSender(
                configuration["Email:Host"],
                configuration.GetValue<int>("Email:Port"),
                configuration["Email:Username"],
                configuration["Email:Password"]
            );
        }

        return services;
    }

    public static IServiceCollection AddSwaggerGenWithAuth(
        this IServiceCollection services,
        string title,
        string description
    )
    {
        return services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = title,
                    Version = "v1",
                    Description = description,
                }
            );

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
}
