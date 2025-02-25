using Domain.Users;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class UserConfiguration : EntityConfiguration<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Status).HasConversion<int>();
    }
}
