using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Configurations;

internal class RecclassConfiguration : IEntityTypeConfiguration<Recclass>
{
    public void Configure(EntityTypeBuilder<Recclass> builder)
    {
        builder
            .HasIndex(r => r.Name)
            .IsUnique();
    }
}
