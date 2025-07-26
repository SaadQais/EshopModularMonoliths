namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregate<Guid>
    {
        public string UserName { get; private set; } = string.Empty;
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        private readonly List<ShoppingCartItem> _items = [];
        public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();

        public static ShoppingCart Create(string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);

            return new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserName = userName
            };
        }

        public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
                _items.Add(newItem);
            }
        }

        public void RemoveItem(Guid productId)
        {
            var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                _items.Remove(existingItem);
            }
        }
    }
}
