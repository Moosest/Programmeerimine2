using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Client
{
    public class ListClientsQuery : IRequest<OperationResult<PagedResult<Client>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}