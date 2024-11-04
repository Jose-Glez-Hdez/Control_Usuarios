using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Usuarios.Models;

public class Access
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    public DateTime DateAccess { get; set; }
    public string? Observations { get; set; }
    public User? User { get; set; }
}
