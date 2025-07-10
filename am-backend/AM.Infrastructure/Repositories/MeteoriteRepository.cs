using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;

namespace AM.Infrastructure.Repositories;

public class MeteoriteRepository(ApplicationDbContext context) : BaseRepository<Meteorite>(context), IMeteoriteRepository
{
}
