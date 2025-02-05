using System.Reflection;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Factories;
using Application.Abstractions.Services;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.Factories;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Services;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Time;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .AddServices()
            .AddSeedData()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();

        return services;
    }

    private static IServiceCollection AddSeedData(this IServiceCollection services)
    {
        IEnumerable<Type> types = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type =>
                type is { IsAbstract: false, IsClass: true }
                && typeof(ISeedEntity).IsAssignableFrom(type)
            );

        foreach (Type type in types)
        {
            services.AddTransient(typeof(ISeedEntity), type);
        }

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                        npgsqlOptions.MigrationsHistoryTable(
                            HistoryRepository.DefaultTableName,
                            Schemas.Default
                        )
                )
                .UseSnakeCaseNamingConvention()
        );

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }

    private static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
                    ),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                };
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        ICacheService cacheService =
                            context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

                        string token = context
                            .Request.Headers.Authorization.ToString()
                            .Replace("Bearer ", string.Empty);

                        if (await cacheService.IsTokenBlacklistedAsync(token))
                        {
                            context.Fail("Token is blacklisted.");
                        }
                    },
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<
            IAuthorizationPolicyProvider,
            PermissionAuthorizationPolicyProvider
        >();

        return services;
    }
}
