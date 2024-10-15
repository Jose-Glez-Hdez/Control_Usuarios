// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Control_Usuarios.Models;

namespace Control_Usuarios.Context
{
    public class AppDbContext : DbContext
    {
        // Constructor que acepta opciones y las pasa a la clase base
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets representan las tablas de la base de datos
        public DbSet<User> Users { get; set; }
    }
}