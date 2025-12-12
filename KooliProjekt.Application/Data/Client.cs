using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(150)]
    public string Email { get; set; }

    [Required]
    [MaxLength(15)]
    public string Phone { get; set; }

    [Required]
    [MaxLength(255)]
    public string Address { get; set; }

    [Required]
    [Range(0.0, 0.9)]
    public decimal Discount { get; set; }
}