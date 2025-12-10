using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Event
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [MaxLength(255)]
    public string Location { get; set; }

    [Required]
    public int MaxSeats { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Summary { get; set; }

    [Required]
    public bool IsActive { get; set; }
}