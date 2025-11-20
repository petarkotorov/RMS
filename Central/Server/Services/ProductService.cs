using Central.API.Data;
using Central.API.Data.Models;
using Central.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

public class ProductService : IProductService
{
    private readonly CentralApplicationDbContext _dbContext;

    public ProductService(CentralApplicationDbContext db)
    {
        _dbContext = db;
    }

    public async Task<List<ProductModel>> GetAllAsync()
    {
        return await _dbContext.Products
            .Where(x => !x.IsDeleted)
            .Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                MinPrice = p.MinPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn,
                SourceStoreId = p.SourceStore,
                DestinationStore = p.DestinationStore
            })
            .ToListAsync();
    }

    public async Task<ProductModel?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products
            .Where(p => p.Id == id && !p.IsDeleted)
            .Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                MinPrice = p.MinPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn,
                SourceStoreId = p.SourceStore,
                DestinationStore = p.DestinationStore,
                IsDeleted = p.IsDeleted
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductModel> CreateAsync(ProductModel model)
    {
        var now = DateTime.UtcNow;

        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            MinPrice = model.MinPrice,
            CreatedOn = now,
            UpdatedOn = now,
            SourceStore = model.SourceStoreId,
            DestinationStore = model.DestinationStore,
            IsDeleted = model.IsDeleted
        };

        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            MinPrice = entity.MinPrice,
            CreatedOn = entity.CreatedOn,
            UpdatedOn = entity.UpdatedOn,
            SourceStoreId = entity.SourceStore,
            DestinationStore = entity.DestinationStore,
            IsDeleted = entity.IsDeleted
        };
    }

    public async Task<ProductModel> UpdateAsync(Guid id, ProductModel model)
    {
        var entity = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Price = model.Price;
        entity.MinPrice = model.MinPrice;
        entity.SourceStore = model.SourceStoreId;
        entity.DestinationStore = model.DestinationStore;
        entity.UpdatedOn = DateTime.UtcNow;
        entity.IsDeleted = model.IsDeleted;

        await _dbContext.SaveChangesAsync();

        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            MinPrice = entity.MinPrice,
            CreatedOn = entity.CreatedOn,
            UpdatedOn = entity.UpdatedOn,
            SourceStoreId = entity.SourceStore,
            DestinationStore = entity.DestinationStore,
            IsDeleted = entity.IsDeleted
        };
    }

    public async Task<ProductModel> DeleteAsync(Guid id)
    {
        var entity = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        entity.IsDeleted = true;
        entity.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        // flags if store to is available
        return new ProductModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            DestinationStore = entity.DestinationStore
        };
    }
}
