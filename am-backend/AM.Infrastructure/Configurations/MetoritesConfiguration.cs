using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Configurations;

internal class MetoritesConfiguration : IEntityTypeConfiguration<Meteorite>
{
    public void Configure(EntityTypeBuilder<Meteorite> builder)
    {

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever();

        builder
            .HasIndex(m => m.Year);

        builder
            .HasIndex(m => m.Name);

        builder
            .HasIndex(m => m.Mass);

        builder
            .HasOne(m => m.Geolocation)
            .WithOne(g => g.Meteorite)
            .HasForeignKey<Geolocation>(g => g.MeteoriteId);

    }
}
