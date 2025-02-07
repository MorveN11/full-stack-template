using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(IApplicationDbContext context) : IPermissionProvider
{
    public async Task<HashSet<string>> GetForUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        HashSet<string> roles = await context
            .UserRoles.AsNoTracking()
            .AsSplitQuery()
            .Include(ur => ur.Role)
            .ThenInclude(r => r.Permissions)
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.Permissions.Select(p => p.Name))
            .ToHashSetAsync(cancellationToken);

        return roles;
    }
}
