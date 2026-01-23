using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class GetSystemUserQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}