using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Payments
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeletePaymentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var payment = await _dbContext.Payments
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (payment == null)
            {
                return result;
            }

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}