using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment 
{
    [Key]
    public int Id { get; set; }

    [Require]
    public int InvoiceId { get; set; }

    [Require]
    public decimal Amount { get; set; }

    [Require]
    public TimeDate PaymentDate { get; set; }

    [Require]
    [MaxLength(50)]
    public string Method { get; set; }

    [Require]
    [MaxLenght(50)]
    public string TransactionRef { get; set; }

    [Require]
    public int ModifiedBy { get; set; }
}