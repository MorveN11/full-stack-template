using Application.Abstractions.Data;

namespace Infrastructure.Seed.Abstractions;

internal interface ISeedEntity
{
    DbPriority Priority { get; }

    void SeedData(IApplicationDbContext context);
}
