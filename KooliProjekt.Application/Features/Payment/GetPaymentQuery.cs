using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.Payments
{
    public class GetPaymentQuery : IRequest<OperationResult<PaymentDetailsDto>>
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; }
        public string TransactionRef { get; set; }
        public int ModifiedBy { get; set; }
    }
}
