using InvoiceDownloader;
using Microsoft.EntityFrameworkCore;

namespace Web
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Invoice> Invoices { get; set; }

        public DbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("Database"));
        }
    }
}
