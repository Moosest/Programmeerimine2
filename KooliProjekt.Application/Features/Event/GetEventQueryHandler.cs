using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Events
{
    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Events
                .Where(e => e.Id == request.Id)
                .Select(e => new
                {
                   e.Id,
                   e.StartTime,
                   e.Description,
                   e.Location,
                   e.MaxSeats,
                   e.Price,
                   e.Summary,
                   e.IsActive,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
