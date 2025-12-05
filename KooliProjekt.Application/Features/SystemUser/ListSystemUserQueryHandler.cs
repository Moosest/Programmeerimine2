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

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class ListSystemUsersQueryHandler : IRequestHandler<ListSystemUsersQuery, OperationResult<IList<SystemUser>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListSystemUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<SystemUser>>> Handle(ListSystemUsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<SystemUser>>();
            result.Value = await _dbContext
                .SystemUsers
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}