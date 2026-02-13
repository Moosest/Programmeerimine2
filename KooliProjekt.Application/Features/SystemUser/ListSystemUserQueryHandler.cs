using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class ListSystemUsersQueryHandler : IRequestHandler<ListSystemUsersQuery, OperationResult<PagedResult<SystemUser>>>
    {
        private readonly ISystemUserRepository _systemUserRepository;

        public ListSystemUsersQueryHandler(ISystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository;
        }

        public async Task<OperationResult<PagedResult<SystemUser>>> Handle(ListSystemUsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<SystemUser>>();

            result.Value = await _systemUserRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
