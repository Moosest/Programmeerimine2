using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(15)]
    public string InvoiceNo { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public decimal Subtotal { get; set; }

    [Required]
    public decimal Shipping { get; set; }

    [Required]
    [Range(0.0, 0.9)]
    public decimal Discount { get; set; }

    [Required]
    public decimal GrandTotal { get; set; }

    public IList<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
}