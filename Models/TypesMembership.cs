using System;

namespace Control_Usuarios.Models;

public class TypesMembership
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Info { get; set; }
    public required decimal Price { get; set; }
    public required int Duration { get; set; }
}
