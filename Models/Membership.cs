using System;

namespace Control_Usuarios.Models;

public class Membership
{
    public int MembershipId { get; set; }
    public int UserId { get; set; }
    public int TypeMembershipId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
