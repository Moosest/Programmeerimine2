using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientsCommandHandler : IRequestHandler<SaveClientsCommand, OperationResult>
    {
        private readonly IClientRepository _clientRepository;

        public SaveClientsCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult> Handle(SaveClientsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Client();
            if(request.Id != 0)
            {
                list = await _clientRepository.GetByIdAsync(request.Id);
            }

            list.Name = request.Name;
            list.Email = request.Email;
            list.Phone = request.Phone;
            list.Address = request.Address;
            list.Discount = request.Discount;

            await _clientRepository.SaveAsync(list);

            return result;
        }
    }
}
