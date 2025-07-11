namespace AM.Application.Models.Common;

public class PagedListResponse<T>
{
    public int PageCount { get; set; }

    public int PageNumber { get; set; }

    public int TotalCount { get; set; }

    public List<T> Items { get; set; }
}