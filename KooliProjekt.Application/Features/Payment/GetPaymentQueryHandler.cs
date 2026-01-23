using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPaymentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Payments
                .Where(payment => payment.Id == request.Id)
                .Select(payment => new
                {
                    payment.Id,
                    payment.InvoiceId,
                    payment.Amount,
                    payment.PaymentDate,
                    payment.Method,
                    payment.TransactionRef,
                    payment.ModifiedBy
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}