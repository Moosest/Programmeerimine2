using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class GetEventScheduleQueryHandler : IRequestHandler<GetEventScheduleQuery, OperationResult<object>>
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;

        public GetEventScheduleQueryHandler(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        public async Task<OperationResult<object>> Handle(GetEventScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var eventSchedule = await _eventScheduleRepository.GetByIdAsync(request.Id);
            if (eventSchedule != null)
            {
                result.Value = new
                {
                    eventSchedule.Id,
                    eventSchedule.EventId,
                    eventSchedule.StartTime,
                    eventSchedule.FilePath,
                    eventSchedule.FileName,
                    eventSchedule.UploadedAt
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