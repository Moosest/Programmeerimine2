namespace KooliProjekt.Application.Dto
{
    public class InvoiceLineItemDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string LineItem { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal VatRate { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool IsDone { get; set; }
    }
}
