public interface ISyncWorkerProcessor
{
    Task ProcessPendingAsync(CancellationToken cancellationToken = default);
}
