using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Usuarios.Models;

public class Membership
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MembershipId { get; set; }
    
    public int UserId { get; set; }
    
    public int TypesMembershipId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; }

    public User? User { get; set; }
    
    public TypesMembership? TypesMembership { get; set; }
}
