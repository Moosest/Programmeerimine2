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

namespace KooliProjekt.Application.Features.Invoices
{
    public class ListInvoicesQueryHandler : IRequestHandler<ListInvoicesQuery, OperationResult<IList<Invoice>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListInvoicesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Invoice>>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Invoice>>();
            result.Value = await _dbContext
                .Invoices
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}