using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Payments
{
    public class SavePaymentCommandHandler : IRequestHandler<SavePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var result = new OperationResult();
            Payment payment;
            if (request.Id == 0)
            {
                payment = new Payment();
                await _dbContext.Payments.AddAsync(payment, cancellationToken);
            }
            else
            {
                payment = await _dbContext.Payments.FindAsync(new object[] { request.Id }, cancellationToken);
                if (payment == null)
                {
                    return result;
                }
            }
            payment.InvoiceId = request.InvoiceId;
            payment.Amount = request.Amount;
            payment.PaymentDate = request.PaymentDate;
            payment.Method = request.Method;
            payment.TransactionRef = request.TransactionRef;
            payment.ModifiedBy = request.ModifiedBy;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}