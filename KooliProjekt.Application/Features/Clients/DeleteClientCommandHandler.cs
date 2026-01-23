using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteClientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.Clients.Where(c => c.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}