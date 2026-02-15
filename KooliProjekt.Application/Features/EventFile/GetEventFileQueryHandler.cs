using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class GetEventFileQueryHandler : IRequestHandler<GetEventFileQuery, OperationResult<EventFileDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventFileQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<EventFileDetailsDto>> Handle(GetEventFileQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<EventFileDetailsDto>();

            if (request.Id <= 0)
            {
                result.Value = new EventFileDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .EventFiles
                .Where(eventFile => eventFile.Id == request.Id)
                .Select(eventFile => new EventFileDetailsDto
                {
                    Id = eventFile.Id,
                    EventId = eventFile.EventId,
                    FilePath = eventFile.FilePath,
                    FileName = eventFile.FileName,
                    UploadedAt = eventFile.UploadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
