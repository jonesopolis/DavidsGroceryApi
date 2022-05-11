using DavidsGrocery.Model;

namespace DavidsGrocery.ViewModels
{
    public class InventoryItemAddViewModel
    {
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }

        public InventoryItem ToInventoryItem()
        {
            return new InventoryItem
            {
                Title = Title,
                Type = Type,
                Description = Description,
                Price = Price,
            };
        }
    }
}
