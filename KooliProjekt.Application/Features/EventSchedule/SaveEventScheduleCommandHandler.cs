using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class SaveEventScheduleCommandHandler : IRequestHandler<SaveEventScheduleCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEventScheduleCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEventScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            EventSchedule eventSchedule;
            if (request.Id == 0)
            {
                eventSchedule = new EventSchedule();
                await _dbContext.EventSchedules.AddAsync(eventSchedule, cancellationToken);
            }
            else
            {
                eventSchedule = await _dbContext.EventSchedules.FindAsync(new object[] { request.Id }, cancellationToken);
                if (eventSchedule == null)
                {
                    return result;
                }
            }
            eventSchedule.EventId = request.EventId;
            eventSchedule.StartTime = request.StartTime;
            eventSchedule.FilePath = request.FilePath;
            eventSchedule.FileName = request.FileName;
            eventSchedule.UploadedAt = request.UploadedAt;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}