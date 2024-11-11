using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Usuarios.Models;

[Table("Users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [StringLength(9)]
    public string? Dni { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }
    [StringLength(50)]
    public string? Surname { get; set; }
    public DateOnly Birthdate { get; set; }
    [StringLength(100)]
    public string? Address { get; set; }
    [StringLength(20)]
    public string? Phone { get; set; }
    [EmailAddress]
    [StringLength(50)]
    public string? Email { get; set; }
    [Required]
    public DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [ForeignKey("Membership")]
    public int MembershipId { get; set; } 
    public TypesMembership? TypesMembership { get; set; }
}
