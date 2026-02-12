using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<object>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice != null)
            {
                result.Value = new
                {
                    invoice.Id,
                    invoice.InvoiceNo,
                    invoice.InvoiceDate,
                    invoice.DueDate,
                    invoice.Subtotal,
                    invoice.Shipping,
                    invoice.Discount,
                    invoice.GrandTotal
                };
            }
            else
            {
                result.Value = null;
            }

            return result;
        }
    }
}