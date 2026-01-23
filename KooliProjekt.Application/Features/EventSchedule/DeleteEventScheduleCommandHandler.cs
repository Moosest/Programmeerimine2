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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.EventSchedules.Where(s => s.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}