using DavidsGrocery.Model;

namespace DavidsGrocery.Repository.Interfaces
{
    public interface IInventoryRepository
    {
        Task<List<InventoryItem>> GetAll();
        Task AddItemToInventory(InventoryItem item);
        Task ResetInventory();
    }
}
