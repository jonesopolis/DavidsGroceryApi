namespace DavidsGrocery.Model
{
    public class CartItem
    {
        public CartItem() : this(default!) { }

        public CartItem(string itemId)
        {
            ItemId = itemId;
            Count = 1;
        }

        public string ItemId { get; set; }
        public int Count { get; set; }
    }
}
