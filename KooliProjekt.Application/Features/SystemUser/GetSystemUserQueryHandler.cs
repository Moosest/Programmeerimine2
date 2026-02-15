using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class GetSystemUserQueryHandler : IRequestHandler<GetSystemUserQuery, OperationResult<SystemUserDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetSystemUserQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<SystemUserDetailsDto>> Handle(GetSystemUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SystemUserDetailsDto>();

            if (request.Id == 0)
            {
                result.Value = new SystemUserDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .SystemUsers
                .Where(systemUser => systemUser.Id == request.Id)
                .Select(systemUser => new SystemUserDetailsDto
                {
                    Id = systemUser.Id,
                    Username = systemUser.Username,
                    PasswordHash = systemUser.PasswordHash,
                    Role = systemUser.Role,
                    CreatedAt = systemUser.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
