using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class DeleteEventScheduleCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}