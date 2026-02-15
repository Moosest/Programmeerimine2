using System;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsDone { get; set; }
    }
}
