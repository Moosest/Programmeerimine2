using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteSystemUserCommand request, CancellationToken cancellationToken)
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

            var user = await _dbContext.SystemUsers
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return result;
            }

            _dbContext.SystemUsers.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}