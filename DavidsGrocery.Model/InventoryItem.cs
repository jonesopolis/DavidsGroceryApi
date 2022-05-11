using System.Text.Json.Serialization;

namespace DavidsGrocery.Model
{
    public class InventoryItem
    {
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string Id { get; set; } = default!;
    }
}