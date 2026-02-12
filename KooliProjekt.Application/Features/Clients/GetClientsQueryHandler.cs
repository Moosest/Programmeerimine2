using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, OperationResult<object>>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientsQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult<object>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var client = await _clientRepository.GetByIdAsync(request.Id);
            if (client != null)
            {
                result.Value = new
                {
                    client.Id,
                    client.Name,
                    client.Email,
                    client.Phone,
                    client.Address,
                    client.Discount
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
