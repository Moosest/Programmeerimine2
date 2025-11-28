using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SystemUser 
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; }

    [Required]
    [MaxLength(20)]
    public string Role { get; set; }

    [Required]
    public TimeDate CreatedAt { get; set; }
}