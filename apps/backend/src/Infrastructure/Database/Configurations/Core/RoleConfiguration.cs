using Domain.Joins;
using Domain.Roles;
using Domain.Users;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class RoleConfiguration : EntityConfiguration<Role>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(r => r.Name).IsUnique();

        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<UserRole>(
                l =>
                    l.HasOne<User>(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired(),
                r =>
                    r.HasOne<Role>(ur => ur.Role)
                        .WithMany(ro => ro.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired()
            );
    }
}
