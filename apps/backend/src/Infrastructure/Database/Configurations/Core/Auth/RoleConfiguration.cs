using Domain.Entities.Auth.Permissions;
using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.Users;
using Domain.Joins;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core.Auth;

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

        builder
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>(
                l =>
                    l.HasOne<Permission>(rp => rp.Permission)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.PermissionId)
                        .IsRequired(),
                r =>
                    r.HasOne<Role>(rp => rp.Role)
                        .WithMany(ro => ro.RolePermissions)
                        .HasForeignKey(rp => rp.RoleId)
                        .IsRequired()
            );
    }
}
