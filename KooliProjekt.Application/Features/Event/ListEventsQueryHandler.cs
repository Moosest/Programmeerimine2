using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, OperationResult<PagedResult<Event>>>
    {
        private readonly IEventRepository _eventRepository;

        public ListEventsQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<OperationResult<PagedResult<Event>>> Handle(ListEventsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Event>>();

            result.Value = await _eventRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
