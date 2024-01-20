using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Entities;

namespace Warehouse.Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser, Role, int>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Client> Clients { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers");

        modelBuilder.Entity<Client>()
            .ToTable("Clients")
            .HasBaseType<ApplicationUser>();

        modelBuilder.Entity<Worker>()
            .ToTable("Workers")
            .HasBaseType<ApplicationUser>();

        modelBuilder.Entity<Client>()
            .HasOne(c => c.ApplicationUser)
            .WithOne(u => u.Client)
            .HasForeignKey<Client>(c => c.ApplicationUserId)
            .IsRequired(false);
        
        modelBuilder.Entity<Worker>()
            .HasOne(w => w.ApplicationUser)
            .WithOne(u => u.Worker)
            .HasForeignKey<Worker>(w => w.ApplicationUserId)
            .IsRequired(false); 
    }
}