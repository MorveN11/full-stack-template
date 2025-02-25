using System.Reflection;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Domain.Services;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.Seed;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Services;
using Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Time;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    ) =>
        services
            .AddServices()
            .AddSeedData()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal()
            .AddCacheInternal(configuration)
            .AddFluentEmailInternal(environment, configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<ICacheService, CacheService>();

        services.AddScoped<IEmailService, EmailService>();

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
                .UseAsyncSeeding(
                    async (dbContext, _, cancellationToken) =>
                    {
                        ServiceProvider serviceProvider = services.BuildServiceProvider();

                        using IServiceScope scope = serviceProvider.CreateScope();

                        IOrderedEnumerable<ISeedEntity> seedEntities = scope
                            .ServiceProvider.GetServices<ISeedEntity>()
                            .OrderBy(entity => entity.Priority);

                        foreach (ISeedEntity seedEntity in seedEntities)
                        {
                            seedEntity.SeedData(dbContext);
                        }

                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                )
                .UseSeeding(
                    (dbContext, _) =>
                    {
                        ServiceProvider serviceProvider = services.BuildServiceProvider();

                        using IServiceScope scope = serviceProvider.CreateScope();

                        IOrderedEnumerable<ISeedEntity> seedEntities = scope
                            .ServiceProvider.GetServices<ISeedEntity>()
                            .Where(entity => entity.Environment == SeedEnvironment.Production)
                            .OrderBy(entity => entity.Priority);

                        foreach (ISeedEntity seedEntity in seedEntities)
                        {
                            seedEntity.SeedData(dbContext);
                        }

                        dbContext.SaveChanges();
                    }
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
        if (!configuration.GetValue<bool>("HealthChecks:Enabled"))
        {
            return services;
        }

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

        services.AddScoped<IPermissionProvider, PermissionProvider>();

        services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();

        return services;
    }

    private static IServiceCollection AddCacheInternal(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");

            options.InstanceName = configuration["Cache:InstanceName"];
        });

        services
            .AddFusionCache()
            .WithDefaultEntryOptions(options =>
                options.Duration = TimeSpan.FromMinutes(
                    configuration.GetValue<int>("Cache:DefaultDurationMinutes")
                )
            )
            .WithSerializer(new FusionCacheSystemTextJsonSerializer())
            .WithDistributedCache(
                new RedisCache(
                    new RedisCacheOptions
                    {
                        Configuration = configuration.GetConnectionString("Redis"),
                    }
                )
            )
            .AsHybridCache();

        return services;
    }

    private static IServiceCollection AddFluentEmailInternal(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration
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
}
