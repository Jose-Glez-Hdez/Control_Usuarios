using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Usuarios.Models;

public class Membership
{
    public int MembershipId { get; set; }
   [ForeignKey("UserId")]
   public int UserId { get; set; }
    [ForeignKey("TypesMembership")]
    public int TypesMembershipId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public User? User { get; set; }
    public TypesMembership? TypesMembership { get; set; }
}
