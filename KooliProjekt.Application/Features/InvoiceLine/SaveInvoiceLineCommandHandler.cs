using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class SaveInvoiceLineCommandHandler : IRequestHandler<SaveInvoiceLineCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceLineCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceLineCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var result = new OperationResult();
            InvoiceLine invoiceLine;
            if (request.Id == 0)
            {
                invoiceLine = new InvoiceLine();
                await _dbContext.InvoiceLines.AddAsync(invoiceLine, cancellationToken);
            }
            else
            {
                invoiceLine = await _dbContext.InvoiceLines.FindAsync(new object[] { request.Id }, cancellationToken);
                if (invoiceLine == null)
                {
                    return result;
                }
            }
            invoiceLine.InvoiceId = request.InvoiceId;
            invoiceLine.LineItem = request.LineItem;
            invoiceLine.UnitPrice = request.UnitPrice;
            invoiceLine.Quantity = request.Quantity;
            invoiceLine.VatRate = request.VatRate;
            invoiceLine.Discount = request.Discount;
            invoiceLine.Total = request.Total;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
