namespace Basket.Data.Repositories
{
    public class BasketRepository(BasketDbContext context) : IBasketRepository
    {
        public async Task<ShoppingCart> GetAsync(
            string userName, 
            bool asNoTracking = true, 
            CancellationToken cancellationToken = default)
        {
            var query = context.ShoppingCarts
                .Include(x => x.Items)
                .Where(x => x.UserName == userName);

            if (asNoTracking)
            {
                query.AsNoTracking();
            }

            var basket = await query.SingleOrDefaultAsync(cancellationToken);

            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<ShoppingCart> CreateAsync(
            ShoppingCart basket, 
            CancellationToken cancellationToken = default)
        {
            context.ShoppingCarts.Add(basket);
            await context.SaveChangesAsync(cancellationToken);

            return basket;
        }

        public async Task<bool> DeleteAsync(
            string userName, 
            CancellationToken cancellationToken = default)
        {
            var basket = await GetAsync(userName, false, cancellationToken);

            context.ShoppingCarts.Remove(basket);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
