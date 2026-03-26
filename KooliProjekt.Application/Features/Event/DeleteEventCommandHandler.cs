using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Events
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteEventCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
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

            // Get the event
            var @event = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (@event == null)
            {
                return result;
            }

            // Delete related EventFiles
            var eventFiles = await _dbContext.EventFiles
                .Where(ef => ef.EventId == request.Id)
                .ToListAsync(cancellationToken);
            
            _dbContext.EventFiles.RemoveRange(eventFiles);

            // Delete related EventSchedules
            var eventSchedules = await _dbContext.EventSchedules
                .Where(es => es.EventId == request.Id)
                .ToListAsync(cancellationToken);
            
            _dbContext.EventSchedules.RemoveRange(eventSchedules);

            // Delete the event
            _dbContext.Events.Remove(@event);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}