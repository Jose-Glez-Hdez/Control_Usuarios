using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Usuarios.Models;

public class Payment
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [ForeignKey("TypeMembershipId")]
    public int TypeMembershipId { get; set; }
    public DateTime DatePay { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public User? User { get; set; }
    public TypesMembership? TypesMembership { get; set; }
}
