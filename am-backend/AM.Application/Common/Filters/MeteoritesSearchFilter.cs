namespace AM.Application.Common.Filters
{
    public class MeteoritesSearchFilter
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public string? Recclass { get; set; }

        public string? NamePart { get; set; }

        public string? SortColumn { get; set; }

        public string? SortOrder { get; set; }
    }
}
