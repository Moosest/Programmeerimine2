using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class DeleteEventScheduleCommandHandler : IRequestHandler<DeleteEventScheduleCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteEventScheduleCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventScheduleCommand request, CancellationToken cancellationToken)
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

            var schedule = await _dbContext.EventSchedules
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schedule == null)
            {
                return result;
            }

            _dbContext.EventSchedules.Remove(schedule);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}