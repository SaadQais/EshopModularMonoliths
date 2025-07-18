namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products =>
        [
            Product.Create("IPhone X", "Long description", "imagefile", 500, ["category1"]),
            Product.Create("Samsung 10", "Long description", "imagefile", 400, ["category1"]),
            Product.Create("Huawei Plus", "Long description", "imagefile", 650, ["category2"]),
            Product.Create("Xiaomi Mi", "Long description", "imagefile", 450, ["category2"])
        ];
    }
}
