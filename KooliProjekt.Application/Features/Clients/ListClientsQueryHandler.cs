using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<PagedResult<Client>>>
    {
        private readonly IClientRepository _clientRepository;

        public ListClientsQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult<PagedResult<Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Client>>();

            result.Value = await _clientRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
