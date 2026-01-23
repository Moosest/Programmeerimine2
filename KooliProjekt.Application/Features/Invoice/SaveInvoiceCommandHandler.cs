using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            Invoice invoice;
            if (request.Id == 0)
            {
                invoice = new Invoice();
                await _dbContext.Invoices.AddAsync(invoice, cancellationToken);
            }
            else
            {
                invoice = await _dbContext.Invoices.FindAsync(new object[] { request.Id }, cancellationToken);
                if (invoice == null)
                {
                    // Optionally handle not found
                    return result;
                }
            }

            invoice.InvoiceNo = request.InvoiceNo;
            invoice.InvoiceDate = request.InvoiceDate;
            invoice.DueDate = request.DueDate;
            invoice.Subtotal = request.Subtotal;
            invoice.Shipping = request.Shipping;
            invoice.Discount = request.Discount;
            invoice.GrandTotal = request.GrandTotal;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}