using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class DeleteSystemUserCommandHandler : IRequestHandler<DeleteSystemUserCommand, OperationResult>
    {
        private readonly ISystemUserRepository _systemUserRepository;

        public DeleteSystemUserCommandHandler(ISystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository;
        }

        public async Task<OperationResult> Handle(DeleteSystemUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _systemUserRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _systemUserRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
