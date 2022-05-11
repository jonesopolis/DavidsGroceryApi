using DavidsGrocery.Model;

namespace DavidsGrocery.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<long> AddItemToCart(string userId, string itemId);
        Task DeleteUserCart(string userId);
        Task<UserCart> GetUserCart(string userId);
        Task<long> RemoveItemFromCart(string userId, string itemId);
        Task ResetCarts();
    }
}
