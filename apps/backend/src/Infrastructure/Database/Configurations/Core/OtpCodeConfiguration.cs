using Domain.OtpCodes;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class OtpCodeConfiguration : EntityConfiguration<OtpCode>
{
    protected override void ConfigureEntity(EntityTypeBuilder<OtpCode> builder)
    {
        builder.Property(r => r.Code).HasMaxLength(6);

        builder.HasIndex(r => r.Code).IsUnique();

        builder.Property(r => r.Type).HasConversion<int>();

        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).IsRequired();
    }
}
