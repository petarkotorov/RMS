using Microsoft.EntityFrameworkCore;
using Shared;
using Store.API.Data;

public class SyncWorkerProcessor : ISyncWorkerProcessor
{
    private readonly StoreApplicationDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public SyncWorkerProcessor(
        StoreApplicationDbContext dbContext,
        IHttpClientFactory httpClientFactory,
        IConfiguration config)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
    {
        var pending = await _dbContext.SyncWorkers
            .Where(x => x.Status == StatusType.Pending)
            .OrderBy(x => x.CreatedOn)
            .ToListAsync(cancellationToken);

        if (!pending.Any())
            return;

        var client = _httpClientFactory.CreateClient("CentralApi");

        foreach (var worker in pending)
        {
            try
            {
                var response = await client.PostAsync(
                    "api/sync",
                    new StringContent(worker.Payload, System.Text.Encoding.UTF8, "application/json"),
                    cancellationToken
                );

                worker.AttemptsCount++;

                if (response.IsSuccessStatusCode)
                {
                    worker.Status = StatusType.Completed;
                }
                else
                {
                    worker.Status = StatusType.Failed;
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                worker.ErrorMessage = ex.Message;
                worker.AttemptsCount++;
                worker.Status = StatusType.Failed;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
