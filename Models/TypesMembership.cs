using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Control_Usuarios.Models;

[Table("TypesMembership")]
public class TypesMembership
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // AUTOINCREMENT for Id
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(200)]
    public string? Info { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
}
