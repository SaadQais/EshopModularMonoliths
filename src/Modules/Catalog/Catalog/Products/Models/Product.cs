namespace Catalog.Products.Models
{
    public class Product : Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string ImageFile { get; private set; } = default!;
        public decimal Price { get; private set; }

        public List<string> Categories { get; private set; } = [];

        public static Product Create(
            string name,
            string description,
            string imageFile,
            decimal price,
            List<string> categories)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

            var product =  new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                ImageFile = imageFile,
                Price = price,
                Categories = categories
            };

            product.AddDomainEvent(new ProductCreatedEvent(product));

            return product;
        }

        public void Update(
            string name,
            string description,
            string imageFile,
            decimal price,
            List<string> categories)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            Name = name;
            Description = description;
            ImageFile = imageFile;
            Categories = categories;

            if(Price != price)
            {
                Price = price;

                AddDomainEvent(new ProductPriceChangedEvent(this));
            }
        }
    }
}
