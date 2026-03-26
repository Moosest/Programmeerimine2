using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
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

            var client = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (client == null)
            {
                return result;
            }

            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}