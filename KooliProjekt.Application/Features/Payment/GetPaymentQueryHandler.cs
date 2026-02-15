using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, OperationResult<PaymentDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPaymentQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PaymentDetailsDto>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PaymentDetailsDto>();

            if (request.Id == 0)
            {
                result.Value = new PaymentDetailsDto();
                return result;
            }

            result.Value = await _dbContext
                .Payments
                .Where(payment => payment.Id == request.Id)
                .Select(payment => new PaymentDetailsDto
                {
                    Id = payment.Id,
                    InvoiceId = payment.InvoiceId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Method = payment.Method,
                    TransactionRef = payment.TransactionRef,
                    ModifiedBy = payment.ModifiedBy
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
