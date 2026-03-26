using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommandValidator : AbstractValidator<SaveEventCommand>
    {
        public SaveEventCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.MaxSeats)
                .GreaterThan(0);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Summary)
                .NotEmpty();
        }
    }
}
