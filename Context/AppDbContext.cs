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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("AutoIncrementBy1").StartsAt(1000).IncrementsBy(1);

        modelBuilder.Entity<User>(entity => // USERS TABLE CONFIGURATION
        {
            entity.HasKey(u => u.Id);
            entity.Property(t => t.Id).UseSequence("AutoIncrementBy1");
            entity.Property(u => u.Dni).IsRequired().HasMaxLength(9);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Birthdate).IsRequired();
            entity.Property(u => u.Address).HasMaxLength(100);
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Email).HasMaxLength(50);
            entity.Property(u => u.RegisterDate).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<TypesMembership>(entity => // TYPESMEMBERSHIPS TABLE CONFIGURATION
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).UseSequence("AutoIncrementBy1");
            entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Info).HasMaxLength(200);
            entity.Property(t => t.Price).HasDefaultValue(0);
            entity.Property(t => t.Duration).HasDefaultValue(0);
        });

        modelBuilder.Entity<Membership>(entity => // MEMBERSHIPS TABLE CONFIGURATION
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).UseSequence("AutoIncrementBy1").IsRequired();
            entity.Property(m => m.TypesMembershipId).IsRequired();
            entity.Property(m => m.StartDate).HasDefaultValueSql("GETDATE()");
            entity.Property(m => m.EndDate).HasDefaultValueSql("DATEADD(day, @Duration, GETDATE())");
            entity.Property(m => m.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Payment>(entity => // PAYMENTS TABLE CONFIGURATION
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).UseSequence("AutoIncrementBy1").IsRequired();
            entity.Property(p => p.UserId).IsRequired();
            entity.Property(p => p.TypeMembershipId).IsRequired();
            entity.Property(p => p.Amount).HasDefaultValue(0);
            entity.Property(p => p.DatePay).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Access>(entity => // ACCESS TABLE CONFIGURATION
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).UseSequence("AutoIncrementBy1").IsRequired();
            entity.Property(a => a.UserId).IsRequired();
            entity.Property(a => a.DateAccess).HasDefaultValueSql("GETDATE()");
        });
    }
    }

    
}