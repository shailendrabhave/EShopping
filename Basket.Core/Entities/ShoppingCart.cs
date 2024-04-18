namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public ShoppingCart(string userName, List<ShoppingCartItem> items)
        {
            UserName = userName;
            Items = items;
        }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
