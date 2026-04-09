using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceLines
{
    public class GetInvoiceLineQuery : IRequest<OperationResult<InvoiceLineDetailsDto>>
    {
        public int Id { get; set; }
    }
}
