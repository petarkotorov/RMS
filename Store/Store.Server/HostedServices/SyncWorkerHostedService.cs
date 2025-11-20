namespace Store.API.HostedServices
{
    public class SyncWorkerHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SyncWorkerHostedService(
            IServiceScopeFactory scopeFactory,
            ILogger<SyncWorkerHostedService> logger)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var processor = scope.ServiceProvider.GetRequiredService<ISyncWorkerProcessor>();

                    await processor.ProcessPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
