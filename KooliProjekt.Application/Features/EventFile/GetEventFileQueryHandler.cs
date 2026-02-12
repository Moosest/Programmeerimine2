using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class GetEventFileQueryHandler : IRequestHandler<GetEventFileQuery, OperationResult<object>>
    {
        private readonly IEventFileRepository _eventFileRepository;

        public GetEventFileQueryHandler(IEventFileRepository eventFileRepository)
        {
            _eventFileRepository = eventFileRepository;
        }

        public async Task<OperationResult<object>> Handle(GetEventFileQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var eventFile = await _eventFileRepository.GetByIdAsync(request.Id);
            if (eventFile != null)
            {
                result.Value = new
                {
                    eventFile.Id,
                    eventFile.EventId,
                    eventFile.FilePath,
                    eventFile.FileName,
                    eventFile.UploadedAt
                };
            }
            else
            {
                result.Value = null;
            }

            return result;
        }
    }
}