using Central.API.Data;
using Central.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Central.API.Services
{
    public class SyncWorkerProcessor : ISyncWorkerProcessor
    {
        private readonly CentralApplicationDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly HashSet<string> _registeredStores;

        public SyncWorkerProcessor(
            CentralApplicationDbContext dbContext,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _config = config;

            _registeredStores = config.GetSection("Stores")
                           .GetChildren()
                           .Select(s => s.Key)
                           .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
        {
            var pending = await _dbContext.SyncWorkers
                .Where(x => x.Status == StatusType.Pending)
                .OrderBy(x => x.CreatedOn)
                .ToListAsync(cancellationToken);

            if (!pending.Any())
                return;


            foreach (var worker in pending)
            {
                if (!_registeredStores.Contains(worker.DestinationStore))
                {
                    worker.Status = StatusType.Failed;
                    worker.AttemptsCount++;
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    continue;
                }

                var client = _httpClientFactory.CreateClient(worker.DestinationStore);

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
}
