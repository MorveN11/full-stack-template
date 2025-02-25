using Domain.UserProfiles;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class UserProfileConfiguration : EntityConfiguration<UserProfile>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserProfile> builder)
    {
        builder
            .HasOne(up => up.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<UserProfile>(up => up.UserId)
            .IsRequired();
    }
}
