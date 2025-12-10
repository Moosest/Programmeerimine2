using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class ListEventsQuery : IRequest<OperationResult<PagedResult<Event>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}