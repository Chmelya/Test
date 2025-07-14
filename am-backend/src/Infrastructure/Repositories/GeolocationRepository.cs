using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;

namespace AM.Infrastructure.Repositories
{
    public class GeolocationRepository(ApplicationDbContext context) : BaseRepository<Geolocation>(context), IGeolocationRepository
    {
    }
}
