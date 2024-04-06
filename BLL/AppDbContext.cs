using InvoiceDownloader;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<AncClassifier> AncClassifiers { get; set; }
        public DbSet<AncClassifierMapping> AncClassifierMappings { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().HasKey(i => new {i.InvoiceNo, i.InvoiceSender, InvoiceDateTime = i.InvoiceDate});
            modelBuilder.Entity<AncClassifierMapping>().HasKey(m => new {m.ProductName});
        }
    }
}
