using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class PaymentDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Method { get; set; }

        public string TransactionRef { get; set; }

        public int ModifiedBy { get; set; }
        public List<PaymentItemDto> Items { get; set; } = new List<PaymentItemDto>();
    }
}
