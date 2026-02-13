using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class ListPaymentsQueryHandler : IRequestHandler<ListPaymentsQuery, OperationResult<PagedResult<Payment>>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public ListPaymentsQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<OperationResult<PagedResult<Payment>>> Handle(ListPaymentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Payment>>();

            result.Value = await _paymentRepository.ListAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
