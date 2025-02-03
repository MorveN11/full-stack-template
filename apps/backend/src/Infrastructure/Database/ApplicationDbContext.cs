using Application.Abstractions.Data;
using Domain.Joins;
using Domain.Roles;
using Domain.Tokens;
using Domain.Users;
using Infrastructure.Seed.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; init; }

    public DbSet<RefreshToken> RefreshTokens { get; init; }

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

    public async Task SeedDataAsync(IServiceScope scope)
    {
        IOrderedEnumerable<ISeedEntity> seedEntities = scope
            .ServiceProvider.GetServices<ISeedEntity>()
            .OrderBy(entity => entity.Priority);

        foreach (ISeedEntity seedEntity in seedEntities)
        {
            seedEntity.SeedData(this);
        }

        await SaveChangesAsync();
    }
}
