using Microsoft.EntityFrameworkCore;
using Store.API.Data.Models;

namespace Store.API.Data
{
    public class StoreApplicationDbContext : DbContext
    {
        public StoreApplicationDbContext(DbContextOptions<StoreApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<SyncWorker> SyncWorkers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
