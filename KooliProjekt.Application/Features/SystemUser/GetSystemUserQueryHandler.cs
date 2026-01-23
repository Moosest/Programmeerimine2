using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class GetSystemUserQueryHandler : IRequestHandler<GetSystemUserQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetSystemUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetSystemUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .SystemUsers
                .Where(systemUser => systemUser.Id == request.Id)
                .Select(systemUser => new
                {
                    systemUser.Id,
                    systemUser.Username,
                    systemUser.PasswordHash,
                    systemUser.Role,
                    systemUser.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}