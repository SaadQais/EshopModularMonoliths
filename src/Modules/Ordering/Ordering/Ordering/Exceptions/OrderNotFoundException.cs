namespace Ordering.Ordering.Exceptions
{
    public class OrderNotFoundException(Guid orderId)
        : NotFoundException("Order", orderId)
    {
    }
}
