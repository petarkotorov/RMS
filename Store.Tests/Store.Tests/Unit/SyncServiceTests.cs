using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Store.API.Data;

namespace Store.Tests.Unit
{
    public class SyncServiceTests
    {
        private StoreApplicationDbContext CreateDb()
        {
            return new StoreApplicationDbContext(
                new DbContextOptionsBuilder<StoreApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
        }

        [Fact]
        public async Task AddSyncEventAsync_ShouldCreateSyncWorker()
        {
            var db = CreateDb();
            var service = new SyncService(db);

            var product = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Desc",
                Price = 2,
                MinPrice = 1,
                DestinationStore = "Lidl Dragalevtsi"
            };

            await service.AddSyncEventAsync(product, ActionType.Created);

            Assert.Single(db.SyncWorkers);
            Assert.Equal(StatusType.Pending, db.SyncWorkers.First().Status);
        }

        [Fact]
        public async Task ProcessSyncEventAsync_ShouldCreateProduct_OnCreated()
        {
            var db = CreateDb();
            var service = new SyncService(db);

            var syncEvent = new Shared.Models.SyncEvent
            {
                Operation = ActionType.Created,
                SourceStore = "Lidl Dragalevtsi",
                Product = new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Desc",
                    Price = 2,
                    MinPrice = 1,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                }
            };

            await service.ProcessSyncEventAsync(syncEvent);

            Assert.Single(db.Products);
            Assert.Equal("Test", db.Products.First().Name);
        }
    }
}
