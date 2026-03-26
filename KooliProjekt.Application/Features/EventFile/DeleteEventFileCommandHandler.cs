using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class DeleteEventFileCommandHandler : IRequestHandler<DeleteEventFileCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteEventFileCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventFileCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var eventFile = await _dbContext.EventFiles
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (eventFile == null)
            {
                return result;
            }

            _dbContext.EventFiles.Remove(eventFile);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}