using DavidsGrocery.Model;
using DavidsGrocery.Repository.Interfaces;
using DavidsGrocery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DavidsGrocery
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _inventoryRepository.GetAll());
        }

        [HttpPost]
        [Authorize(Policy = "Shopkeep")]
        public async Task<IActionResult> AddInventory([FromBody] InventoryItemAddViewModel vm)
        {
            await _inventoryRepository.AddItemToInventory(vm.ToInventoryItem());
            return Ok();
        }

        [HttpDelete]
        [Authorize(Policy = "Shopkeep")]
        public async Task<IActionResult> Delete()
        {
            await _inventoryRepository.ResetInventory();
            return Ok();
        }
    }
}