using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class InvoiceLine
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InvoiceId { get; set; }

    [Required]
    [MaxLength(255)]
    public string LineItem { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public decimal VatRate { get; set; }

    [Required]
    [Range(0.0, 0.9)]
    public decimal Discount { get; set; }

    [Required]
    public decimal Total { get; set; }

    public Invoice Invoice { get; set; }
}