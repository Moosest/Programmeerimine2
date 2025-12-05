using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<IList<Client>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListClientsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Clients>>();
            result.Value = await _dbContext
                .Client
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}