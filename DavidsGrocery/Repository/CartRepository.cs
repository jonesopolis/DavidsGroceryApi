using DavidsGrocery.Models;

namespace DavidsGrocery.Repository
{
    public class CartRepository
    {
        private List<UserCart> _carts;

        public CartRepository()
        {
            _carts = new List<UserCart>();
        }

        public UserCart GetUserCart(string userId)
        {
            var cart = _carts.FirstOrDefault(x => x.UserId == userId);

            if(cart == null)
            {
                cart = new UserCart(userId);
                _carts.Add(cart);
            }

            return cart;
        }

        public long AddItemToCart(string userId, long itemId)
        {
            var cart = GetUserCart(userId);

            var cartItem = cart.Cart.FirstOrDefault(x => x.ItemId == itemId);

            if(cartItem == null)
            {
                cartItem = new CartItem(itemId);
                cart.Cart.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            return cart.Cart.Sum(x => x.Count);
        }

        public long RemoveItemFromCart(string userId, long itemId)
        {
            var cart = GetUserCart(userId);

            var cartItem = cart.Cart.FirstOrDefault(x => x.ItemId == itemId);

            if (cartItem != null)
            {             
                cart.Cart.Remove(cartItem);
            }

            return cart.Cart.Sum(x => x.Count);
        }

        public void DeleteUserCart(string userId)
        {
            var cart = _carts.FirstOrDefault(x => x.UserId == userId);

            if(cart != null)
            {
                _carts.Remove(cart);
            }
        }
    }
}
