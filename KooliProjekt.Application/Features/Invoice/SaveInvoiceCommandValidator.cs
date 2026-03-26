using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
    {
        public SaveInvoiceCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.InvoiceNo)
                .NotEmpty()
                .MaximumLength(15);

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(x => x.InvoiceDate);

            RuleFor(x => x.Subtotal)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Shipping)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Discount)
                .InclusiveBetween(0, 0.9m);

            RuleFor(x => x.GrandTotal)
                .GreaterThanOrEqualTo(0);
        }
    }
}
