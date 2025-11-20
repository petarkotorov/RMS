using Central.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;

namespace Central.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ISyncService _syncService;

        public ProductsController(IProductService productService, ISyncService syncService)
        {
            _productService = productService;
            _syncService = syncService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _productService.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null 
                ? NotFound() 
                : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductModel model)
        {
            var created = await _productService.CreateAsync(model);
            if (!string.IsNullOrEmpty(model.DestinationStore))
            {
                await _syncService.AddSyncEventAsync(created, ActionType.Created);
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductModel model)
        {
            var updated = await _productService.UpdateAsync(id, model);
            if (!string.IsNullOrEmpty(model.DestinationStore))
            {
                await _syncService.AddSyncEventAsync(updated, ActionType.Updated);
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productModel = await _productService.DeleteAsync(id);

            // only creates event when destination store is available
            if (!string.IsNullOrEmpty(productModel.DestinationStore))
            {
                await _syncService.AddSyncEventAsync(productModel, ActionType.Deleted);
            }

            return NoContent();
        }
    }
}
