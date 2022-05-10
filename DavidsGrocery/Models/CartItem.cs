namespace DavidsGrocery.Models
{
    public class CartItem
    {
        public CartItem(long itemId)
        {
            ItemId = itemId;
            Count = 1;
        }

        public long ItemId { get; set; }
        public int Count { get; set; }
    }
}
