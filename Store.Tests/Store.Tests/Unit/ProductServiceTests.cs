using Store.API.Data;
using Store.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Store.Tests.Unit
{
    public class ProductServiceTests
    {
        private StoreApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<StoreApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new StoreApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct()
        {
            var db = CreateDbContext();
            var service = new ProductService(db);

            var model = new ProductModel
            {
                Name = "ABC",
                Description = "ABC DESC",
                Price = 2,
                MinPrice = 1
            };

            var result = await service.CreateAsync(model);

            Assert.NotNull(result);
            Assert.Equal("ABC", result.Name);
            Assert.Single(db.Products);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            var db = CreateDbContext();
            var service = new ProductService(db);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "BCA",
                Description = "BCA DESC",
                Price = 10,
                MinPrice = 5,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();

            var updateModel = new ProductModel
            {
                Name = "New",
                Description = "New Desc",
                Price = 20,
                MinPrice = 10,
                SourceStoreId = "Lidl Dragalevtsi"
            };

            var result = await service.UpdateAsync(product.Id, updateModel);

            Assert.Equal("New", result.Name);
            Assert.Equal(20, result.Price);
        }
    }
}
