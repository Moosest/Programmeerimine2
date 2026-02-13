using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, OperationResult>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _clientRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _clientRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
