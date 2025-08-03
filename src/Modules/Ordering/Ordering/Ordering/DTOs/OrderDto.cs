namespace Ordering.Ordering.DTOs
{
    public record OrderDto(
        Guid Id,
        Guid CustomerId,
        string OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        List<OrderItemDto> Items
    );
}
