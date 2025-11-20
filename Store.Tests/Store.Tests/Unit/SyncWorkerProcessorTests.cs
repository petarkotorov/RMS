using Store.API.Data;
using Store.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using Shared;

namespace Store.Tests.Unit
{
    public class SyncWorkerProcessorTests
    {
        private StoreApplicationDbContext CreateDb()
        {
            return new StoreApplicationDbContext(
                new DbContextOptionsBuilder<StoreApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options);
        }

        [Fact]
        public async Task ProcessPendingAsync_ShouldMarkCompleted_WhenPostSucceeds()
        {
            var db = CreateDb();

            db.SyncWorkers.Add(new SyncWorker
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Action = ActionType.Created,
                AttemptsCount = 0,
                Status = StatusType.Pending,
                Payload = "{}",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var messageHandler = new Mock<HttpMessageHandler>();
            messageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));


            var httpClient = new HttpClient(messageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var clientFactory = new Mock<IHttpClientFactory>();
            clientFactory.Setup(x => x.CreateClient("CentralApi")).Returns(httpClient);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                { "CentralApi", "http://localhost" }
                })
                .Build();

            var processor = new SyncWorkerProcessor(db, clientFactory.Object, config);

            await processor.ProcessPendingAsync();

            var worker = db.SyncWorkers.First();

            Assert.Equal(StatusType.Completed, worker.Status);
            Assert.Equal(1, worker.AttemptsCount);
        }
    }
}
