namespace AM.Application.Common.Filters
{
    public class MeteoritesSearchFilter
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public string? Recclass { get; set; }

        public string? NamePart { get; set; }

        public string? SortColumn { get; set; }

        public string? SortOrder { get; set; }
    }
}
