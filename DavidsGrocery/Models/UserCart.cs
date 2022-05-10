namespace DavidsGrocery.Models
{
    public class UserCart
    {
        public UserCart(string userId)
        {
            UserId = userId;
            Cart = new List<CartItem>();
        }

        public string UserId { get; set; }
        public List<CartItem> Cart { get; set; }
    }
}
