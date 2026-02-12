using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using KooliProjekt.Application.Data;

public class EventFile : Entity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EventId { get; set; }

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }

    [Required]
    public DateTime UploadedAt { get; set; }
}