using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public SaveInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoice = new Invoice();
            if (request.Id != 0)
            {
                invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            }

            invoice.InvoiceNo = request.InvoiceNo;
            invoice.InvoiceDate = request.InvoiceDate;
            invoice.DueDate = request.DueDate;
            invoice.Subtotal = request.Subtotal;
            invoice.Shipping = request.Shipping;
            invoice.Discount = request.Discount;
            invoice.GrandTotal = request.GrandTotal;

            await _invoiceRepository.SaveAsync(invoice);

            return result;
        }
    }
}
