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

namespace KooliProjekt.Application.Features.Payments
{
    public class ListPaymentsQueryHandler : IRequestHandler<ListPaymentsQuery, OperationResult<PagedResult<Payment>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListPaymentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Payment>>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Payment>>();

            result.Value = await _dbContext
                .Payments
                .OrderBy(p => p.PaymentDate)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}