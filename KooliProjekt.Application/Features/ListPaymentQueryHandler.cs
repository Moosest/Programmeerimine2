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

namespace KooliProjekt.Application.Features.Payments
{
    public class ListPaymentsQueryHandler : IRequestHandler<ListPaymentsQuery, OperationResult<IList<Payment>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListPaymentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Payment>>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Payment>>();
            result.Value = await _dbContext
                .Payments
                .OrderBy(list => list.Name)
                .ToListAsync();

            return result;
        }
    }
}