using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] AllowedSortByValues =
    {
        "title",
        "yearofrelease"
    };
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField)
            .Must(sortField => string.IsNullOrWhiteSpace(sortField) ||
                               AllowedSortByValues.Contains(sortField.ToLower()))
            .WithMessage($"SortBy must be one of the following values: {string.Join(", ", AllowedSortByValues)}");

        RuleFor(x => x.PageNumber)
            .InclusiveBetween(1, 25)
            .WithMessage("PageNumber must be between 1 and 25.");
    }
}