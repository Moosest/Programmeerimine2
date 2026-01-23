using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class GetEventFileQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}