using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Seed.Abstractions;

internal abstract class SeedEntity<T>(DbPriority priority = DbPriority.One) : ISeedEntity
    where T : Entity
{
    protected abstract IEnumerable<T> GetData();

    public DbPriority Priority { get; } = priority;

    public void SeedData(IApplicationDbContext context)
    {
        DbSet<T> dbSet = context.Set<T>();
        IEnumerable<T> seedData = GetData();

        var existingIds = dbSet.Select(entity => entity.Id).ToHashSet();
        IEnumerable<T> entitiesToAdd = seedData.Where(entity => !existingIds.Contains(entity.Id));

        dbSet.AddRange(entitiesToAdd);
    }
}
