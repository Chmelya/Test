using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Infrastructure.Configurations;

public class MetoritesConfiguration : IEntityTypeConfiguration<Meteorite>
{
    public void Configure(EntityTypeBuilder<Meteorite> builder)
    {

        builder
            .Property(m => m.Id)
            .ValueGeneratedNever(); 
        

        builder
            .HasOne(m => m.Geolocation)
            .WithOne(g => g.Meteorite)
            .HasForeignKey<Geolocation>(g => g.MeteoriteId);
    }
}
