using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class invoice 
{
    [Key]
    public int Id { get; set; }

    [Require]
    public int RegistrationId { get; set; }

    [Require]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; }

    [Require]
    public decimal Amount { get; set; }

    [Require]
    public TimeDate IssueDate  { get; set; }

    [Require]
    public TimeDate DueDate { get; set; }

    [Require]
    public string Status { get; set; }

    [Require]
    public string PdfUrl { get; set; }
}