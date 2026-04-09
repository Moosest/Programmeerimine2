using FluentValidation;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class SaveInvoiceLineCommandValidator : AbstractValidator<SaveInvoiceLineCommand>
    {
        public SaveInvoiceLineCommandValidator()
        {
            RuleFor(x => x.InvoiceId).GreaterThan(0);
            RuleFor(x => x.LineItem).NotEmpty().MaximumLength(255);
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.VatRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Discount).InclusiveBetween(0, 0.9m);
            RuleFor(x => x.Total).GreaterThanOrEqualTo(0);
        }
    }
}
