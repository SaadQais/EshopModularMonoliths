namespace Basket.Data.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetAsync(
            string userName, 
            bool asNoTracking = true, 
            CancellationToken cancellationToken = default);

        Task<ShoppingCart> CreateAsync(
            ShoppingCart shoppingCart, 
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            string userName, 
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
