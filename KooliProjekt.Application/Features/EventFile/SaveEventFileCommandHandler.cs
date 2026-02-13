using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class SaveEventFileCommandHandler : IRequestHandler<SaveEventFileCommand, OperationResult>
    {
        private readonly IEventFileRepository _eventFileRepository;

        public SaveEventFileCommandHandler(IEventFileRepository eventFileRepository)
        {
            _eventFileRepository = eventFileRepository;
        }

        public async Task<OperationResult> Handle(SaveEventFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var eventFile = new EventFile();
            if (request.Id != 0)
            {
                eventFile = await _eventFileRepository.GetByIdAsync(request.Id);
            }

            eventFile.EventId = request.EventId;
            eventFile.FilePath = request.FilePath;
            eventFile.FileName = request.FileName;
            eventFile.UploadedAt = request.UploadedAt;

            await _eventFileRepository.SaveAsync(eventFile);

            return result;
        }
    }
}
