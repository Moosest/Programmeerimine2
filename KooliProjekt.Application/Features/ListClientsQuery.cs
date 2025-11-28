using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Client
{
    public class ListClientListsQuery : IRequest<OperationResult<IList<Client>>>
    {
    }
}