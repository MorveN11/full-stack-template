using Domain.Tokens;
using Infrastructure.Database.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Core;

internal sealed class EmailVerificationTokenConfiguration
    : EntityConfiguration<EmailVerificationToken>
{
    protected override void ConfigureEntity(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).IsRequired();
    }
}
