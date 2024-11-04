// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Control_Usuarios.Models;

namespace Control_Usuarios.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TypesMembership> TypesMemberships { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Access> Access { get; set; }
    }
}