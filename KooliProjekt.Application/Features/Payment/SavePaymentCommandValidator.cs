using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandValidator : AbstractValidator<SavePaymentCommand>
    {
        public SavePaymentCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.InvoiceId)
                .GreaterThan(0);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Method)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.TransactionRef)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.ModifiedBy)
                .GreaterThan(0);
        }
    }
}
