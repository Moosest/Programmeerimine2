using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class GetEventScheduleQueryHandler : IRequestHandler<GetEventScheduleQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventScheduleQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetEventScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .EventSchedules
                .Where(eventSchedule => eventSchedule.Id == request.Id)
                .Select(eventSchedule => new
                {
                    eventSchedule.Id,
                    eventSchedule.EventId,
                    eventSchedule.StartTime,
                    eventSchedule.FilePath,
                    eventSchedule.FileName,
                    eventSchedule.UploadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}