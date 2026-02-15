using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Events
{
    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, OperationResult<EventDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetEventQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<EventDetailsDto>> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<EventDetailsDto>();

            if (request.Id <= 0)
            {
                result.Value = new EventDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .Events
                .Where(e => e.Id == request.Id)
                .Select(e => new EventDetailsDto
                {
                   Id = e.Id,
                   StartTime = e.StartTime,
                   Description = e.Description,
                   Location = e.Location,
                   MaxSeats = e.MaxSeats,
                   Price = e.Price,
                   Summary = e.Summary,
                   IsActive = e.IsActive,
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
