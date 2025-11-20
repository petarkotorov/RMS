namespace Central.API.Services.Interfaces
{
    public interface ISyncWorkerProcessor
    {
        Task ProcessPendingAsync(CancellationToken cancellationToken = default);
    }
}
