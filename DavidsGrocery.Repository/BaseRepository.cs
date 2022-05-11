using Microsoft.Azure.Cosmos;

namespace DavidsGrocery.Repository
{
    public abstract class BaseRepository<T>
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseId;
        private readonly string _containerName;
        private readonly string _partitionKey;

        protected readonly Container _container;

        public BaseRepository(CosmosClient cosmosClient, 
                              string databaseId, 
                              string containerName,
                              string partitionKey)
        {
            _databaseId = databaseId;
            _containerName = containerName;
            _partitionKey = partitionKey;

            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(databaseId, containerName);
        }

        protected async Task RebuildContainer()
        {
            await _container.DeleteContainerAsync();
            await _cosmosClient.GetDatabase(_databaseId)
                               .CreateContainerAsync(_containerName, _partitionKey);
        }

        protected Task DeleteItem(string id, string partitionKeyValue)
        {
            return _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKeyValue));
        }
    }
}
