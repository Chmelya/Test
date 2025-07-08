using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repositories;

public class MeteoriteRepository(ApplicationDbContext context) : BaseRepository<Meteorite>(context), IMeteoriteRepository
{
    public async Task BulkInsertAsync(IEnumerable<Meteorite> incomingEntities)
    {
        var meteoritesDict = incomingEntities.ToDictionary(_ => _.Name);
        var existingIds = meteoritesDict.Keys.ToList();

        var dbSet = Context.Meteorites;

        var existingRecords = await dbSet
            .Where(_ => existingIds.Contains(_.Name))
            .ToListAsync();

        var toAdd = new List<Meteorite>();

        foreach (var meteorite in meteoritesDict.Values)
        {
            var existing = existingRecords.FirstOrDefault(m => m.Id == meteorite.Id);
            if (existing != null)
            {
                Context.Entry(existing).CurrentValues.SetValues(meteorite);
            }
            else
            {
                toAdd.Add(meteorite);
            }
        }

        if (toAdd.Any())
        {
            await dbSet.AddRangeAsync(toAdd);
        }

        await Context.SaveChangesAsync();
    }
}
