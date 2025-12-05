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

namespace KooliProjekt.Application.Features.EventFiles
{
    public class ListEventFilesQueryHandler : IRequestHandler<ListEventFilesQuery, OperationResult<IList<EventFile>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListEventFilesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<EventFile>>> Handle(ListEventFilesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<EventFile>>();
            result.Value = await _dbContext
                .EventFiles
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}