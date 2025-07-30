namespace Basket.Basket.DTOs
{
    public record ShoppingCartDto(
        Guid Id,
        string UserName,
        decimal TotalPrice,
        List<ShoppingCartItemDto> Items);
}
