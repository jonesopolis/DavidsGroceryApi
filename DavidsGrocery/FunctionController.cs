using DavidsGrocery.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DavidsGrocery
{
    [ApiController]
    [Route("[controller]")]
    public class FunctionController : ControllerBase
    {
        private readonly IHubContext<InventoryHub> _inventoryHub;

        public FunctionController(IHubContext<InventoryHub> inventoryHub)
        {
            _inventoryHub = inventoryHub;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InventoryItem inventoryItem)
        {
            await _inventoryHub.Clients.All.SendAsync("inventory-added", inventoryItem);
            return Ok();
        }
    }
}