using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Store.API.Data;
using Store.API.Data.Models;
using Store.API.Services.Interfaces;
using System.Text.Json;

public class SyncService : ISyncService
{
    private readonly StoreApplicationDbContext _dbContext;

    public SyncService(StoreApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSyncEventAsync(ProductModel product, string operation)
    {
        var now = DateTime.UtcNow;

        var syncEvent = new SyncEvent
        {
            Operation = operation,
            Product = product,
            SourceStore = "Lidl Dragalevtsi",
            TimeStamp = now,
            DestinationStore = "Central"
        };

        var payload = JsonSerializer.Serialize(syncEvent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var worker = new SyncWorker
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Action = operation,
            Status = StatusType.Pending,
            AttemptsCount = 0,
            Payload = payload,
            CreatedOn = now
        };

        await _dbContext.SyncWorkers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ProcessSyncEventAsync(SyncEvent syncEvent)
    {
        if (syncEvent == null)
        {
            return;
        }

        if (syncEvent.Product == null)
        {
            return;
        }

        var payload = syncEvent.Product;

        var existing = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == payload.Id);

        switch (syncEvent.Operation)
        {
            case ActionType.Created:
                await HandleCreatedAsync(payload, existing);
                break;

            case ActionType.Updated:
                await HandleUpdatedAsync(payload, existing);
                break;

            case ActionType.Deleted:
                await HandleDeletedAsync(payload.Id, existing);
                break;

            default:
                break;
        }
    }

    private async Task HandleCreatedAsync(
        ProductModel model,
        Product? existing)
    {
        if (existing != null)
        {
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.Price = model.Price;
            existing.MinPrice = model.MinPrice;
            existing.UpdatedOn = DateTime.UtcNow;
            existing.IsDeleted = false;

            await _dbContext.SaveChangesAsync();
            return;
        }

        var entity = new Product
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            MinPrice = model.MinPrice,
            CreatedOn = model.CreatedOn,
            UpdatedOn = model.UpdatedOn,
            IsDeleted = false
        };

        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    private async Task HandleUpdatedAsync(
        ProductModel model,
        Product? existing)
    {
        if (existing == null)
        {
            await HandleCreatedAsync(model, null);
            return;
        }

        existing.Name = model.Name;
        existing.Description = model.Description;
        existing.Price = model.Price;
        existing.MinPrice = model.MinPrice;
        existing.UpdatedOn = DateTime.UtcNow;
        existing.IsDeleted = model.IsDeleted;

        await _dbContext.SaveChangesAsync();
    }

    private async Task HandleDeletedAsync(Guid id, Product? existing)
    {
        if (existing == null)
        {
            return;
        }

        existing.IsDeleted = true;
        existing.UpdatedOn = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }
}
