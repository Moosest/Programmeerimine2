using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoiceCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}