using System;

namespace Control_Usuarios.Models;

public class Payment
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required DateTime DatePay { get; set; }
    public required decimal Amount { get; set; }
    public required string PaymentMethod { get; set; }
    public required bool IsPaid { get; set; }
}
