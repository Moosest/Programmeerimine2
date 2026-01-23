using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Invoices
                .Where(invoice => invoice.Id == request.Id)
                .Select(invoice => new
                {
                    invoice.Id,
                    invoice.InvoiceNo,
                    invoice.InvoiceDate,
                    invoice.DueDate,
                    invoice.Subtotal,
                    invoice.Shipping,
                    invoice.Discount,
                    invoice.GrandTotal
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}