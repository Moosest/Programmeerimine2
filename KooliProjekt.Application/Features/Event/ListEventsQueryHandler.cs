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

namespace KooliProjekt.Application.Features.Events
{
    public class ListEventsQueryHandler : IRequestHandler<ListEventsQuery, OperationResult<PagedResult<Event>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEventsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Event>>> Handle(ListEventsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Event>>();

            result.Value = await _dbContext
                .Events
                .OrderBy(e => e.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}