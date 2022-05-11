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

        public async Task<List<InventoryItem>> GetAll()
        {
            var iterator = _container.GetItemQueryIterator<InventoryItem>();

            var list = new List<InventoryItem>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                list.AddRange(response);
            }

            return list;
        }

        public async Task AddItemToInventory(InventoryItem item)
        {
            var iterator = _container.GetItemQueryIterator<string>("SELECT VALUE MAX(c.id) FROM c");
            var response = await iterator.ReadNextAsync();

            item.Id = (int.Parse(response.First()) + 1).ToString();

            await _container.UpsertItemAsync(item);
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
