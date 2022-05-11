using DavidsGrocery.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DavidsGrocery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public IActionResult Get()
        {
            var json = System.IO.File.ReadAllText("inventory.json");
            return Ok(json);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _inventoryRepository.ResetInventory();
            return Ok();
        }
    }
}