using Domain.Joins;
using Domain.OtpCodes;
using Domain.Permissions;
using Domain.RefreshTokens;
using Domain.Roles;
using Domain.UserProfiles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<OtpCode> OtpCodes { get; }
    DbSet<UserProfile> UserProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
