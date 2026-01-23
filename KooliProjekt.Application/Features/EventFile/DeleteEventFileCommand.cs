using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class DeleteEventFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}