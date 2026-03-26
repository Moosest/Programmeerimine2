using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientsCommandValidator : AbstractValidator<SaveClientsCommand>
    {
        public SaveClientsCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(150)
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(15);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Discount)
                .InclusiveBetween(0, 0.9m);
        }
    }
}
