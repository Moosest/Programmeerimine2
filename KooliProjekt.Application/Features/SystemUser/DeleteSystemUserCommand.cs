using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class DeleteSystemUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}