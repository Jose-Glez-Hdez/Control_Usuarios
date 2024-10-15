using System;

namespace Control_Usuarios.Models;

public class Access
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DateAccess { get; set; }
    public string? Observations { get; set; }
}
