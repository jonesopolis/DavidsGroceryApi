using DavidsGrocery.Model;
using DavidsGrocery.Repository.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace DavidsGrocery.Repository
{   
    public class CartRepository : BaseRepository<UserCart>, ICartRepository
    {
        public CartRepository(CosmosClient cosmosClient) : base(cosmosClient,
                                                                "DavidsGrocery",
                                                                "Cart",
                                                                "/userId")
        {
        }

        public async Task<long> AddItemToCart(string userId, string itemId)
        {
            var userCart = await GetUserCart(userId);

            var cartItem = userCart.Cart.FirstOrDefault(x => x.ItemId == itemId);
            if (cartItem == null)
            {
                userCart.Cart.Add(new CartItem(itemId));
            }
            else
            {
                cartItem.Count++;
            }

            await _container.UpsertItemAsync(userCart);
            return userCart.Cart.Sum(x => x.Count);
        }

        public async Task<long> RemoveItemFromCart(string userId, string itemId)
        {
            var userCart = await GetUserCart(userId);

            var cartItem = userCart.Cart.FirstOrDefault(x => x.ItemId == itemId);
            if (cartItem != null)
            {
                userCart.Cart.Remove(cartItem);
            }

            await _container.UpsertItemAsync(userCart);
            return userCart.Cart.Sum(x => x.Count);
        }

        public Task DeleteUserCart(string userId)
        {
            return DeleteItem(userId, userId);
        }

        public async Task<UserCart> GetUserCart(string userId)
        {
            var iterator = await _container.GetItemLinqQueryable<UserCart>()
                                          .Where(p => p.UserId == userId)
                                          .ToFeedIterator()
                                          .ReadNextAsync();

            var userCart = iterator.FirstOrDefault();

            if (userCart == null)
            {
                userCart = new UserCart(userId);
                await _container.CreateItemAsync(userCart);
            }

            return userCart;
        }

        public Task ResetCarts()
        {
            return RebuildContainer();
        }
    }
}
