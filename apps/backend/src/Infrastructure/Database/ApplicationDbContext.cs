using Application.Abstractions.Data;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.Permissions;
using Domain.Entities.Auth.RefreshTokens;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.UserProfiles;
using Domain.Entities.Auth.Users;
using Domain.Joins;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IPublisher publisher
) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<UserRole> UserRoles { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<RolePermission> RolePermissions { get; init; }
    public DbSet<Permission> Permissions { get; init; }
    public DbSet<RefreshToken> RefreshTokens { get; init; }
    public DbSet<OtpCode> OtpCodes { get; init; }
    public DbSet<UserProfile> UserProfiles { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        IList<IDomainEvent> domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IList<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
