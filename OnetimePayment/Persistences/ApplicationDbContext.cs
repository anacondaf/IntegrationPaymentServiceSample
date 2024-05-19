using Microsoft.EntityFrameworkCore;

namespace OnetimePayment;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Film> Films { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder
            .Entity<Order>()
            .HasMany(o => o.Films)
            .WithMany(f => f.Orders);

        builder
            .Entity<Film>()
            .HasMany(f => f.Prices)
            .WithOne(p => p.Film)
            .HasForeignKey(p => p.FilmId);

        builder
            .Entity<Film>()
            .HasMany(f => f.Orders)
            .WithMany(o => o.Films);

        builder
            .Entity<FilmPrice>()
            .HasOne(p => p.Film)
            .WithMany(f => f.Prices);

        builder
            .Entity<Transaction>()
            .HasOne(t => t.Order)
            .WithOne(o => o.Transaction);

        builder
            .Entity<Order>()
            .HasOne(o => o.Transaction)
            .WithOne(t => t.Order);

        base.OnModelCreating(builder);
    }
}