using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class GetEventScheduleQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}