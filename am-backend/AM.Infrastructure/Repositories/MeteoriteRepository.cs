using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Repositories;
using AM.Application.Common.Responses;
using AM.Domain.Enities;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace AM.Infrastructure.Repositories;

public class MeteoriteRepository(ApplicationDbContext context) : BaseRepository<Meteorite>(context), IMeteoriteRepository
{
    public async Task<IPagedList<MeteoritesGropedResponse>> GetGroupedByYearPagedAsync(
        MeteoritesSearchFilter filter,
        bool isAsNoTracking = false)
    {
        var query = GetAsSplitable(isAsNoTracking);

        query = ApplyFilters(query, filter);

        var responseQuery = query
            .GroupBy(x => x.Year)
            .Select(g => new MeteoritesGropedResponse()
            {
                Year = g.Key,
                TotalMass = g.Sum(x => x.Mass),
                MeteoritesCount = g.Count()
            });

        var pagedList = await responseQuery.ToPagedListAsync(filter.PageNumber, filter.PageSize);

        return pagedList;
    }

    private static IQueryable<Meteorite> ApplyFilters(IQueryable<Meteorite> query, MeteoritesSearchFilter filter)
    {
        query = FilterQuery(query, filter);

        var orderSelector = GetOrderSelector(filter.SortColumn);

        query = ApplySort(query, filter.SortOrder, orderSelector);

        return query;
    }

    private static IQueryable<Meteorite> FilterQuery(IQueryable<Meteorite> query, MeteoritesSearchFilter filter)
    {
        if (filter.StartYear is not null)
        {
            query = query.Where(m => m.Year >= filter.StartYear);
        }

        if (filter.EndYear is not null)
        {
            query = query.Where(m => m.Year <= filter.EndYear);
        }

        if (filter.Recclass is not null)
        {
            query = query.Where(m => m.Recclass == filter.Recclass);
        }

        if (filter.NamePart is not null)
        {
            query = query.Where(m => m.Name.Contains(filter.NamePart));
        }

        return query;
    }


    private static Expression<Func<Meteorite, object>> GetOrderSelector(string? sortColumn)
    {
        var propertyName = string.IsNullOrEmpty(sortColumn)
            ? nameof(Meteorite.Name)
            : sortColumn;

        var param = Expression.Parameter(typeof(Meteorite));
        return Expression.Lambda<Func<Meteorite, object>>(
            Expression.Convert(Expression.Property(param, propertyName), typeof(object)),
            param);
    }

    private static IQueryable<Meteorite> ApplySort(
        IQueryable<Meteorite> query, string? sortOrder,
        Expression<Func<Meteorite, object>> orderSelector)
    {
        return (sortOrder?.ToUpperInvariant() ?? "ASC") == "DESC"
            ? query.OrderByDescending(orderSelector)
            : query.OrderBy(orderSelector);
    }
}
