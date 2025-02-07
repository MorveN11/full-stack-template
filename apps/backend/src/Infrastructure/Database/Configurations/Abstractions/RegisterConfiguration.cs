using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace Infrastructure.Database.Configurations.Abstractions;

internal abstract class RegisterConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Register
{
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        _ = builder
            .Property(e => e.CreatedOnUtc)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        _ = builder
            .Property(e => e.UpdatedOnUtc)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnUpdate();

        ConfigureEntity(builder);
    }
}
