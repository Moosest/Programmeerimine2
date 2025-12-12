using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Paging;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class ListEventSchedulesQuery : IRequest<OperationResult<PagedResult<EventSchedule>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}