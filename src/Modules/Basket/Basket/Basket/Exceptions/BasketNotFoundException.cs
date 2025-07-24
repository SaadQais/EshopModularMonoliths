namespace Basket.Basket.Exceptions
{
    public class BasketNotFoundException(string userName) : NotFoundException("Basket", userName)
    {
    }
}
