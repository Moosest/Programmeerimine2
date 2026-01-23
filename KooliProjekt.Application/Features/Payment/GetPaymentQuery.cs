using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}