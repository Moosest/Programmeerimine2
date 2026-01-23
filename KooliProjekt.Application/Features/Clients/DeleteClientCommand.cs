using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class DeleteClientCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}