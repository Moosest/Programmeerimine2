using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class GetEventFileQueryHandler : IRequestHandler<GetEventFileQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventFileQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetEventFileQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .EventFiles
                .Where(eventFile => eventFile.Id == request.Id)
                .Select(eventFile => new
                {
                    eventFile.Id,
                    eventFile.EventId,
                    eventFile.FilePath,
                    eventFile.FileName,
                    eventFile.UploadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}