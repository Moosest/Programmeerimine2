using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class ListEventSchedulesQueryHandler : IRequestHandler<ListEventSchedulesQuery, OperationResult<PagedResult<EventSchedule>>>
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;

        public ListEventSchedulesQueryHandler(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        public async Task<OperationResult<PagedResult<EventSchedule>>> Handle(ListEventSchedulesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<EventSchedule>>();

            result.Value = await _eventScheduleRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
