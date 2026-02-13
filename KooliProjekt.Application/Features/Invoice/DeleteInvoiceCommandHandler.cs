using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, OperationResult>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _invoiceRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _invoiceRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
