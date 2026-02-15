using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<InvoiceDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceDetailsDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceDetailsDto>();

            if (request.Id == 0)
            {
                result.Value = new InvoiceDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .Invoices
                .Where(invoice => invoice.Id == request.Id)
                .Select(invoice => new InvoiceDetailsDto
                {
                    Id = invoice.Id,
                    InvoiceNo = invoice.InvoiceNo,
                    InvoiceDate = invoice.InvoiceDate,
                    DueDate = invoice.DueDate,
                    Subtotal = invoice.Subtotal,
                    Shipping = invoice.Shipping,
                    Discount = invoice.Discount,
                    GrandTotal = invoice.GrandTotal
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
