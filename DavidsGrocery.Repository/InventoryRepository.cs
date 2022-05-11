using DavidsGrocery.Model;
using DavidsGrocery.Repository.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace DavidsGrocery.Repository
{
    public class InventoryRepository : BaseRepository<InventoryItem>, IInventoryRepository
    {
        public InventoryRepository(CosmosClient cosmosClient) : base(cosmosClient, 
                                                                     "DavidsGrocery", 
                                                                     "Inventory", 
                                                                     "/id")
        {
        }

        public async Task ResetInventory()
        {
            await RebuildContainer();

            var json = File.ReadAllText("inventory.json");
            var items = JsonSerializer.Deserialize<List<InventoryItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var item in items)
            {
                await _container.CreateItemAsync(item);
            }
        }
    }
}
