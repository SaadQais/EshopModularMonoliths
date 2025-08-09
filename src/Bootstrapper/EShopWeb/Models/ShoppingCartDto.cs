namespace EShopWeb.Models
{
    public record ShoppingCartDto(
        Guid Id,
        string UserName,
        decimal TotalPrice,
        List<ShoppingCartItemDto> Items);

    public record ShoppingCartItemDto(
        Guid Id,
        Guid ShoppingCartId,
        Guid ProductId,
        int Quantity,
        string Color,
        decimal Price,
        string ProductName);
}
