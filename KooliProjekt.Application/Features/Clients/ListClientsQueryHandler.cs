using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<PagedResult<Client>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListClientsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Client>>();

            result.Value = await _dbContext
                .Clients
                .OrderBy(c => c.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}