using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, OperationResult<object>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<OperationResult<object>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var payment = await _paymentRepository.GetByIdAsync(request.Id);
            if (payment != null)
            {
                result.Value = new
                {
                    payment.Id,
                    payment.InvoiceId,
                    payment.Amount,
                    payment.PaymentDate,
                    payment.Method,
                    payment.TransactionRef,
                    payment.ModifiedBy
                };
            }
            else
            {
                result.Value = null;
            }

            return result;
        }
    }
}