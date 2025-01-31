using Domain.Roles;
using Domain.Tokens;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<EmailVerificationToken> EmailVerificationTokens { get; }

    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
