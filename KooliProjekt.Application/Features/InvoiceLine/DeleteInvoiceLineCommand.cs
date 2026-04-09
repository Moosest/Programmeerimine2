using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class DeleteInvoiceLineCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
