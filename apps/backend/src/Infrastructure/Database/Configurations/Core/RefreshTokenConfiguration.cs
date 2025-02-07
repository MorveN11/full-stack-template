using Domain.Tokens;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class RefreshTokenConfiguration : EntityConfiguration<RefreshToken>
{
    protected override void ConfigureEntity(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(r => r.Token).HasMaxLength(200);

        builder.HasIndex(r => r.Token).IsUnique();

        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).IsRequired();
    }
}
