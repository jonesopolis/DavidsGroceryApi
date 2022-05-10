using DavidsGrocery.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DavidsGrocery.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartRepository _cartRepository;

        public CartController(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var cart = _cartRepository.GetUserCart(User.Identity.Name);
            return Ok(cart.Cart);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ItemViewModel item)
        {
            var totalCount = _cartRepository.AddItemToCart(User.Identity.Name, item.ItemId);

            return Ok(totalCount);
        }

        [HttpPut]
        public IActionResult Remove([FromBody] ItemViewModel item)
        {
            var totalCount = _cartRepository.RemoveItemFromCart(User.Identity.Name, item.ItemId);
            return Ok(totalCount);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            _cartRepository.DeleteUserCart(User.Identity.Name);
            return Ok();
        }
    }

    public class ItemViewModel
    {
        public long ItemId { get; set; }
    }
}