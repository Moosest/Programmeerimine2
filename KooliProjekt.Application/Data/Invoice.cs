using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class invoice 
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RegistrationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime IssueDate  { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public string PdfUrl { get; set; }
}