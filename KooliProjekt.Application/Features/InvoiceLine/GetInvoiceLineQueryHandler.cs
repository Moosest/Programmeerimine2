using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQueryHandler : IRequestHandler<GetInvoiceLineQuery, OperationResult<InvoiceLineDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceLineQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceLineDetailsDto>> Handle(GetInvoiceLineQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<InvoiceLineDetailsDto>();

            if (request.Id <= 0)
            {
                result.Value = new InvoiceLineDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .InvoiceLines
                .Where(il => il.Id == request.Id)
                .Select(il => new InvoiceLineDetailsDto
                {
                    Id = il.Id,
                    InvoiceId = il.InvoiceId,
                    LineItem = il.LineItem,
                    UnitPrice = il.UnitPrice,
                    Quantity = il.Quantity,
                    VatRate = il.VatRate,
                    Discount = il.Discount,
                    Total = il.Total
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
