using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceLineDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }

        public string LineItem { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal VatRate { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
        public List<InvoiceLineItemDto> Items { get; set; } = new List<InvoiceLineItemDto>();
    }
}
