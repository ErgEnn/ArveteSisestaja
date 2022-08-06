using Domain;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace DAL;

public class AppDbContext : DbContext
{
    private const string ConnectionString = "Server=localhost;Database=anc_importer;Port=6033;Uid=anc_user";
    public static AppDbContext Instance = new AppDbContext();

    public DbSet<AncIngredient> AncIngredients { get; set; }
    public DbSet<ItemAncClassifier> ItemAncRelations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, new MariaDbServerVersion(new Version(10, 8, 3)));
    }
}