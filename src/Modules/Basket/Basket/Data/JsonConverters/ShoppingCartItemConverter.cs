namespace Basket.Data.JsonConverters
{
    public class ShoppingCartItemConverter : JsonConverter<ShoppingCartItem>
    {
        public override ShoppingCartItem? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDocument.RootElement;

            var id = rootElement.GetProperty("Id").GetGuid();
            var shoppingCartId = rootElement.GetProperty("ShoppingCartId").GetGuid();
            var productId = rootElement.GetProperty("ProductId").GetGuid();
            var quantity = rootElement.GetProperty("Quantity").GetInt32();
            var color = rootElement.GetProperty("Color").GetString()!;
            var price = rootElement.GetProperty("Price").GetDecimal();
            var productName = rootElement.GetProperty("ProductName").GetString()!;

            return new ShoppingCartItem(id, shoppingCartId, productId, quantity, color, price, productName);
        }

        public override void Write(
            Utf8JsonWriter writer, 
            ShoppingCartItem value, 
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Id", value.Id.ToString());
            writer.WriteString("ShoppingCartId", value.ShoppingCartId.ToString());
            writer.WriteString("ProductId", value.ProductId.ToString());
            writer.WriteNumber("Quantity", value.Quantity);
            writer.WriteString("Color", value.Color);
            writer.WriteNumber("Price", value.Price);
            writer.WriteString("ProductName", value.ProductName);

            writer.WriteEndObject();
        }
    }
}
