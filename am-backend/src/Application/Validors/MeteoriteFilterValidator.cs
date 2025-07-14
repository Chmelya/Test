using AM.Application.Common.Filters;
using AM.Domain.Enities;
using FluentValidation;

namespace AM.Application.Validors
{
    internal class MeteoriteFilterValidator : AbstractValidator<MeteoritesSearchFilter>
    {
        public MeteoriteFilterValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be higher 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be higher 0");

            RuleFor(x => x.StartYear)
                .GreaterThanOrEqualTo(0)
                .When(x => x.StartYear.HasValue)
                .WithMessage("Start year must be higher 0");

            RuleFor(x => x.EndYear)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .When(x => x.EndYear.HasValue)
                .WithMessage($"End year musn't be higher than {DateTime.Now.Year}");

            RuleFor(x => x)
                .Must(x => x.EndYear == null || x.StartYear == null || x.EndYear >= x.StartYear)
                .WithMessage("Год окончания не может быть раньше года начала");

            RuleFor(x => x.NamePart)
                .MaximumLength(50).WithMessage("Name cannot be longer 50 characters")
                .Matches(@"^[a-zA-Z0-9\s\-]*$").When(x => !string.IsNullOrEmpty(x.NamePart))
                .WithMessage("Name contains forbiden characters");

            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn).When(x => !string.IsNullOrEmpty(x.SortColumn))
                .WithMessage("No such sort column");

            RuleFor(x => x.SortOrder)
                .Must(BeAValidSortOrder).When(x => !string.IsNullOrEmpty(x.SortOrder))
                .WithMessage("Sort order can be only 'ASC or 'DESC'");
        }

        private bool BeAValidSortColumn(string? column)
        {
            if (column is null)
            {
                return true;
            }
            ;

            var validColumns = new[] {
                nameof(Meteorite.Name),
                nameof(Meteorite.RecclassId),
                nameof(Meteorite.Mass),
                nameof(Meteorite.Reclong),
                nameof(Meteorite.Reclat)
            };

            return validColumns.Contains(column, StringComparer.OrdinalIgnoreCase);
        }

        private bool BeAValidSortOrder(string? order)
        {
            if (order is null) 
            {
                return true;
            };

            return order.ToUpperInvariant().Equals("ASC")
                || order.ToUpperInvariant().Equals("DESC");
        }
    }
}
