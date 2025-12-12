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

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class ListEventSchedulesQueryHandler : IRequestHandler<ListEventSchedulesQuery, OperationResult<PagedResult<EventSchedule>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListEventSchedulesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<EventSchedule>>> Handle(ListEventSchedulesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<EventSchedule>>();

            result.Value = await _dbContext
                .EventSchedules
                .OrderBy(es => es.StartTime)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}