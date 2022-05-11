using DavidsGrocery.Repository;
using DavidsGrocery.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DavidsGrocery.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cart = await _cartRepository.GetUserCart(User.Identity.Name);
            return Ok(cart.Cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemViewModel item)
        {
            var totalCount = await _cartRepository.AddItemToCart(User.Identity.Name, item.ItemId);
            return Ok(totalCount);
        }

        [HttpPut]
        public async Task<IActionResult> Remove([FromBody] ItemViewModel item)
        {
            var totalCount = await _cartRepository.RemoveItemFromCart(User.Identity.Name, item.ItemId);
            return Ok(totalCount);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _cartRepository.DeleteUserCart(User.Identity.Name);
            return Ok();
        }

        [HttpGet("superdelete")]
        public async Task<IActionResult> SuperDelete()
        {
            await _cartRepository.ResetCarts();
            return Ok();
        }
    }

    public class ItemViewModel
    {
        public string ItemId { get; set; }
    }
}