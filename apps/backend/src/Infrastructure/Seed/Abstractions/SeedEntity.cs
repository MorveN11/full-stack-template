using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Infrastructure.Seed.Abstractions;

internal abstract class SeedEntity<T>(
    DbPriority priority = DbPriority.One,
    SeedEnvironment environment = SeedEnvironment.Development
) : ISeedEntity
    where T : Entity
{
    protected abstract IEnumerable<T> GetData();

    public DbPriority Priority { get; } = priority;

    public SeedEnvironment Environment { get; } = environment;

    public void SeedData(DbContext context)
    {
        DbSet<T> dbSet = context.Set<T>();

        IEnumerable<T> seedData = GetData();

        var existingIds = dbSet.Select(entity => entity.Id).ToHashSet();

        T[] entitiesToAdd = seedData.Where(entity => !existingIds.Contains(entity.Id)).ToArray();

        if (entitiesToAdd.Any())
        {
            dbSet.AddRange(entitiesToAdd);
        }
    }
}
