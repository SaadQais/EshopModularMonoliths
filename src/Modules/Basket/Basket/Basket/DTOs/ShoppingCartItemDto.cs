﻿namespace Basket.Basket.DTOs
{
    public record ShoppingCartItemDto(
        Guid Id,
        Guid ShoppingCartId,
        Guid ProductId,
        int Quantity,
        string Color,
        decimal Price,
        string ProductName);
}
