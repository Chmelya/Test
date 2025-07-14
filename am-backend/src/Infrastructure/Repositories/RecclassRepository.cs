using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;

namespace AM.Infrastructure.Repositories
{
    public class RecclassRepository(ApplicationDbContext context) : BaseRepository<Recclass>(context), IRecclassRepository
    {
    }
}
