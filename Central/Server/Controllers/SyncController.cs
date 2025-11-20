using Central.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Central.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult> ImportFromStore([FromBody] SyncEvent syncEvent)
        {
            if (syncEvent == null)
            {
                return BadRequest("Sync data cannot be null");
            }

            await _syncService.ProcessSyncEventAsync(syncEvent);

            return Ok(new
            {
                Message = "Sync data processed successfully",
                syncEvent.Operation,
                syncEvent.Product?.Id
            });
        }
    }
}
