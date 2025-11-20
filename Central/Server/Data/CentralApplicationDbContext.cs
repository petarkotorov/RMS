using Microsoft.EntityFrameworkCore;
using Central.API.Data.Models;

namespace Central.API.Data
{
    public class CentralApplicationDbContext : DbContext
    {
        public CentralApplicationDbContext(DbContextOptions<CentralApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<SyncWorker> SyncWorkers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
