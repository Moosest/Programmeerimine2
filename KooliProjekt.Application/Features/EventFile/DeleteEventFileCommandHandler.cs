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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteEventFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.EventFiles.Where(f => f.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}