using Shared.Models;

namespace Store.API.Services.Interfaces
{
    public interface ISyncService
    {
        Task AddSyncEventAsync(ProductModel product, string operation);

        Task ProcessSyncEventAsync(SyncEvent syncEvent);
    }
}
