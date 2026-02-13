using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class ListEventFilesQueryHandler : IRequestHandler<ListEventFilesQuery, OperationResult<PagedResult<EventFile>>>
    {
        private readonly IEventFileRepository _eventFileRepository;

        public ListEventFilesQueryHandler(IEventFileRepository eventFileRepository)
        {
            _eventFileRepository = eventFileRepository;
        }

        public async Task<OperationResult<PagedResult<EventFile>>> Handle(ListEventFilesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<EventFile>>();

            result.Value = await _eventFileRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
