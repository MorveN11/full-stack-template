using Domain.Roles;
using Infrastructure.Seed.Abstractions;
using Infrastructure.Seed.Core.Ids;
using SharedKernel.Time;

namespace Infrastructure.Seed.Core.Data;

internal sealed class RolesData(IDateTimeProvider timeProvider) : SeedEntity<Role>
{
    protected override IEnumerable<Role> GetData()
    {
        return
        [
            new Role
            {
                Id = RolesId.User,
                Name = "User",
                CreatedAt = timeProvider.UtcNow,
            },
            new Role
            {
                Id = RolesId.Admin,
                Name = "Admin",
                CreatedAt = timeProvider.UtcNow,
            },
        ];
    }
}
