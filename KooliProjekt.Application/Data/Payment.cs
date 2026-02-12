using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KooliProjekt.Application.Data;

public class Payment : Entity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InvoiceId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Method { get; set; }

    [Required]
    [MaxLength(50)]
    public string TransactionRef { get; set; }

    [Required]
    public int ModifiedBy { get; set; }
}