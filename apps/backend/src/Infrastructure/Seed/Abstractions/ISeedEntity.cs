using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed.Abstractions;

internal interface ISeedEntity
{
    DbPriority Priority { get; }

    SeedEnvironment Environment { get; }

    void SeedData(DbContext context);
}
