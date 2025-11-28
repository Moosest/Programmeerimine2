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
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(150)]
    public string Email { get; set; }

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; }

    [Required]
    [MaxLength(150)]
    public string CompanyName { get; set; }
}