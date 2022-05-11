namespace DavidsGrocery.Model
{
    public class UserCart
    {
        public UserCart() : this(default!) { }

        public UserCart(string userId) 
        {
            UserId = userId;
            Cart = new List<CartItem>();
        }

        public string Id => UserId;
        public string UserId { get; set; }
        public List<CartItem> Cart { get; set; }
    }
}
