using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class DeleteSystemUserCommandHandler : IRequestHandler<DeleteSystemUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteSystemUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteSystemUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            await _dbContext.SystemUsers.Where(u => u.Id == request.Id).ExecuteDeleteAsync(cancellationToken);
            return result;
        }
    }
}