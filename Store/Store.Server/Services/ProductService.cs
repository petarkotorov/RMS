using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Store.API.Data;
using Store.API.Data.Models;
using Store.API.Services.Interfaces;

public class ProductService : IProductService
{
    private readonly StoreApplicationDbContext _dbContext;

    public ProductService(StoreApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductModel>> GetAllAsync()
    {
        return await _dbContext.Products
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                MinPrice = p.MinPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn
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
                UpdatedOn = p.UpdatedOn
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductModel> CreateAsync(ProductModel model)
    {
        var now = DateTime.UtcNow;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            MinPrice = model.MinPrice,
            CreatedOn = now,
            UpdatedOn = now,
            IsDeleted = false
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            MinPrice = product.MinPrice,
            CreatedOn = product.CreatedOn,
            UpdatedOn = product.UpdatedOn
        };
    }

    public async Task<ProductModel> UpdateAsync(Guid id, ProductModel model)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.MinPrice = model.MinPrice;
        product.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            MinPrice = product.MinPrice,
            CreatedOn = product.CreatedOn,
            UpdatedOn = product.UpdatedOn
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        product.IsDeleted = true;
        product.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }
}
