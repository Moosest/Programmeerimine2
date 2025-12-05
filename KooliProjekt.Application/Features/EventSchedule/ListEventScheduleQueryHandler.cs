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

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class ListEventSchedulesQueryHandler : IRequestHandler<ListEventSchedulesQuery, OperationResult<IList<EventSchedule>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListEventSchedulesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<EventSchedule>>> Handle(ListEventSchedulesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<EventSchedule>>();
            result.Value = await _dbContext
                .EventSchedules
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}