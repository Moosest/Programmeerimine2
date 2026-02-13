using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandHandler : IRequestHandler<SavePaymentCommand, OperationResult>
    {
        private readonly IPaymentRepository _paymentRepository;

        public SavePaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<OperationResult> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var payment = new Payment();
            if (request.Id != 0)
            {
                payment = await _paymentRepository.GetByIdAsync(request.Id);
            }

            payment.InvoiceId = request.InvoiceId;
            payment.Amount = request.Amount;
            payment.PaymentDate = request.PaymentDate;
            payment.Method = request.Method;
            payment.TransactionRef = request.TransactionRef;
            payment.ModifiedBy = request.ModifiedBy;

            await _paymentRepository.SaveAsync(payment);

            return result;
        }
    }
}
