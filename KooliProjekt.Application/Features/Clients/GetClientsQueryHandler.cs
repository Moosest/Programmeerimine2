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
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, OperationResult<ClientListDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetClientsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Clients
                .Where(client => client.Id == request.Id)
                .Select(client => new
                {
                    client.Id,
                    client.Name,
                    client.Email,
                    client.Phone,
                    client.Address,
                    client.Discount
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
