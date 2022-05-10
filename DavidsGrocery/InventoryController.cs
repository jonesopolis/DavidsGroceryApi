using Microsoft.AspNetCore.Mvc;

namespace DavidsGrocery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        public InventoryController()
        {

        }

        public IActionResult Get()
        {
            var json = System.IO.File.ReadAllText("inventory.json");
            return Ok(json);
        }
    }
}