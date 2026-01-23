using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class DeleteEventCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}