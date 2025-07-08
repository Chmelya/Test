using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Configurations;

public class MetoritesConfiguration : IEntityTypeConfiguration<Meteorite>
{
    public void Configure(EntityTypeBuilder<Meteorite> builder)
    {
        builder
            .HasIndex(m => m.Name)
            .IsUnique();
    }
}
