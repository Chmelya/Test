using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Repositories;
using AM.Application.Common.Responses;
using AM.Application.Models.Common;
using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AM.Infrastructure.Repositories;

public class MeteoriteRepository(ApplicationDbContext context) : BaseRepository<Meteorite>(context), IMeteoriteRepository
{
    public async Task<PagedListResponse<MeteoritesGropedResponse>> GetGroupedByYearPagedAsync(
        MeteoritesSearchFilter filter,
        bool isAsNoTracking = false)
    {
        var query = GetAsSplitable(isAsNoTracking); 

        query = FilterQuery(query, filter);

        var orderSelector = GetOrderSelector(filter.SortColumn);

        query = ApplySort(query, filter.SortOrder, orderSelector);

        var totalCount = await query.CountAsync();

        var responseQuery = query
            .Include(m => m.Recclass)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .GroupBy(x => x.Year)
            .Select(g => new MeteoritesGropedResponse()
            {
                Year = g.Key,
                TotalMass = g.Sum(x => x.Mass),
                MeteoritesCount = g.Count(),
                Meteorites = g.ToList(),
            });


        var items = await responseQuery.ToListAsync();

        var response = new PagedListResponse<MeteoritesGropedResponse>()
        {
            PageNumber = filter.PageNumber,
            PageCount = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
            TotalCount = totalCount,
            Items = items,
        };

        return response;
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
            query = query.Where(m => m.RecclassId == filter.Recclass);
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
            ? nameof(Meteorite.Year)
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
