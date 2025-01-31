using Application.Abstractions.Data;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(IApplicationDbContext context)
{
    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        User user =
            await context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == userId)
            ?? throw new InvalidOperationException("User not found.");

        var permissionsSet = user.Roles.Select(r => r.Name).ToHashSet();

        return permissionsSet;
    }
}
