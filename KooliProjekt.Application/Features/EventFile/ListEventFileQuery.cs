using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class ListEventFilesQuery : IRequest<OperationResult<PagedResult<EventFile>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}