using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class GetSystemUserQueryHandler : IRequestHandler<GetSystemUserQuery, OperationResult<object>>
    {
        private readonly ISystemUserRepository _systemUserRepository;

        public GetSystemUserQueryHandler(ISystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository;
        }

        public async Task<OperationResult<object>> Handle(GetSystemUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var systemUser = await _systemUserRepository.GetByIdAsync(request.Id);
            if (systemUser != null)
            {
                result.Value = new
                {
                    systemUser.Id,
                    systemUser.Username,
                    systemUser.PasswordHash,
                    systemUser.Role,
                    systemUser.CreatedAt
                };
            }
            else
            {
                result.Value = null;
            }

            return result;
        }
    }
}