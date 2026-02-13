using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class SaveEventScheduleCommandHandler : IRequestHandler<SaveEventScheduleCommand, OperationResult>
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;

        public SaveEventScheduleCommandHandler(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        public async Task<OperationResult> Handle(SaveEventScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var eventSchedule = new EventSchedule();
            if (request.Id != 0)
            {
                eventSchedule = await _eventScheduleRepository.GetByIdAsync(request.Id);
            }

            eventSchedule.EventId = request.EventId;
            eventSchedule.StartTime = request.StartTime;
            eventSchedule.FilePath = request.FilePath;
            eventSchedule.FileName = request.FileName;
            eventSchedule.UploadedAt = request.UploadedAt;

            await _eventScheduleRepository.SaveAsync(eventSchedule);

            return result;
        }
    }
}
