using System;

namespace KooliProjekt.Application.Dto
{
    public class PaymentItemDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; }
        public string TransactionRef { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsDone { get; set; }
    }
}
