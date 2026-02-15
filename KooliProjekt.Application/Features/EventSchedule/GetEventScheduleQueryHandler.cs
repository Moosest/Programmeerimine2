using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class GetEventScheduleQueryHandler : IRequestHandler<GetEventScheduleQuery, OperationResult<EventScheduleDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventScheduleQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<EventScheduleDetailsDto>> Handle(GetEventScheduleQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<EventScheduleDetailsDto>();

            if (request.Id == 0)
            {
                result.Value = new EventScheduleDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .EventSchedules
                .Where(eventSchedule => eventSchedule.Id == request.Id)
                .Select(eventSchedule => new EventScheduleDetailsDto
                {
                    Id = eventSchedule.Id,
                    EventId = eventSchedule.EventId,
                    StartTime = eventSchedule.StartTime,
                    FilePath = eventSchedule.FilePath,
                    FileName = eventSchedule.FileName,
                    UploadedAt = eventSchedule.UploadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
