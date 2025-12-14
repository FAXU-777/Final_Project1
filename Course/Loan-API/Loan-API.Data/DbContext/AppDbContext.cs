namespace Loan_API.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Serilog;





public class AppDbContext : DbContext
{
    public  AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Modeles.User> Users { get; set; }
    public DbSet<Modeles.Loan> Loans { get; set; }
    
 //   public DbSet<Log> Logs { get; set; }
 
 
    
    // Configure the model relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Modeles.User>()
            .HasMany(u => u.Loans)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        base.OnModelCreating(modelBuilder);

    }
    
}