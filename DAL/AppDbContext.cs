using Domain;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DAL;

public class AppDbContext : DbContext
{
    private const string ConnectionString = "Server=localhost;Database=anc;Port=6033;Uid=anc";
    public static AppDbContext Instance = new AppDbContext();

    public DbSet<AncIngredient> AncIngredients { get; set; }
    public DbSet<ItemAncClassifier> ItemAncRelations { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, new MariaDbServerVersion(new Version(10, 8, 3)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Invoice>().HasKey(invoice => new
        {
            invoice.Vendor,
            invoice.Identifier
        });
    }
}