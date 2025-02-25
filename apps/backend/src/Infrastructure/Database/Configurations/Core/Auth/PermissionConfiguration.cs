using Domain.Entities.Auth.Permissions;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core.Auth;

internal sealed class PermissionConfiguration : EntityConfiguration<Permission>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Permission> builder)
    {
        builder.HasIndex(r => r.Name).IsUnique();
    }
}
