using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, OperationResult<ClientDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetClientsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<ClientDetailsDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<ClientDetailsDto>();

            if (request.Id <= 0)
            {
                result.Value = new ClientDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .Clients
                .Where(client => client.Id == request.Id)
                .Select(client => new ClientDetailsDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Phone = client.Phone,
                    Address = client.Address,
                    Discount = client.Discount
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
