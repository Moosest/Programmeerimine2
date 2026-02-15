using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Shipping { get; set; }

        public decimal Discount { get; set; }

        public decimal GrandTotal { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
    }
}
