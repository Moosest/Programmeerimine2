using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class SaveSystemUserCommandHandler : IRequestHandler<SaveSystemUserCommand, OperationResult>
    {
        private readonly ISystemUserRepository _systemUserRepository;

        public SaveSystemUserCommandHandler(ISystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository;
        }

        public async Task<OperationResult> Handle(SaveSystemUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var user = new SystemUser();
            if (request.Id != 0)
            {
                user = await _systemUserRepository.GetByIdAsync(request.Id);
            }

            user.Username = request.Username;
            user.PasswordHash = request.PasswordHash;
            user.Role = request.Role;
            user.CreatedAt = request.CreatedAt;

            await _systemUserRepository.SaveAsync(user);

            return result;
        }
    }
}
