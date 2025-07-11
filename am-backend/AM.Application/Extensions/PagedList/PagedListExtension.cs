using AM.Application.Models.Common;
using X.PagedList;

namespace AM.Application.Extensions.PagedList;

public static class PagedListExtension
{
    public static PagedListResponse<T> ToPagedResponse<T>(this IPagedList<T> list)
    {
        return new PagedListResponse<T>()
        {
            PageCount = list.PageCount,
            PageNumber = list.PageNumber,
            TotalCount = list.TotalItemCount,
            Items = [.. list],
        };
    }
}
