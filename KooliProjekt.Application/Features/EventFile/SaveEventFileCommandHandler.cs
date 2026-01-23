using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class SaveEventFileCommandHandler : IRequestHandler<SaveEventFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEventFileCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEventFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            EventFile eventFile;
            if (request.Id == 0)
            {
                eventFile = new EventFile();
                await _dbContext.EventFiles.AddAsync(eventFile, cancellationToken);
            }
            else
            {
                eventFile = await _dbContext.EventFiles.FindAsync(new object[] { request.Id }, cancellationToken);
                if (eventFile == null)
                {
                    return result;
                }
            }
            eventFile.EventId = request.EventId;
            eventFile.FilePath = request.FilePath;
            eventFile.FileName = request.FileName;
            eventFile.UploadedAt = request.UploadedAt;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}