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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.Events.Where(e => e.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}