using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Control_Usuarios.Models;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // AUTOINCREMENT for Id
    public int Id {get; set;}
    [Required]
    [StringLength(9)]
    public required string Dni {get; set;}
    [Required]
    [StringLength(50)]
    public required string Name {get; set;}
    [Required]
    [StringLength(50)]
    public required string Surname {get; set;}
    public DateOnly Birthdate {get; set;}
    [Required]
    [StringLength(100)]
    public string? Address {get; set;}
    [StringLength(20)]
    public string? Phone {get; set;}
    [EmailAddress]
    [StringLength(50)]
    public string? Email {get; set;}
    [Required]
    public DateOnly RegisterDate {get; set;} = DateOnly.FromDateTime(DateTime.Now);

}
