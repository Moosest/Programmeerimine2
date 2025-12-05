using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class ListEventSchedulesQuery : IRequest<OperationResult<IList<EventSchedule>>>
    {
    }
}