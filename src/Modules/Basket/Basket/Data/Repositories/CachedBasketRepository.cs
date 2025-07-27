namespace Basket.Data.Repositories
{
    public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) 
        : IBasketRepository
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new ShoppingCartConverter(),
                new ShoppingCartItemConverter()
            }
        };

        public async Task<ShoppingCart> GetAsync(
            string userName, 
            bool asNoTracking = true, 
            CancellationToken cancellationToken = default)
        {
            if(!asNoTracking)
                return await basketRepository.GetAsync(userName, asNoTracking, cancellationToken);

            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
            }
                
            var basket = await basketRepository.GetAsync(userName, asNoTracking, cancellationToken);

            cache.SetString(
                userName, 
                JsonSerializer.Serialize(basket, _options));

            return basket;
        }

        public async Task<ShoppingCart> CreateAsync(
            ShoppingCart basket, 
            CancellationToken cancellationToken = default)
        {
            await basketRepository.CreateAsync(basket, cancellationToken);

            cache.SetString(
                basket.UserName,
                JsonSerializer.Serialize(basket, _options));

            return basket;
        }

        public async Task<bool> DeleteAsync(
            string userName, 
            CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteAsync(userName, cancellationToken);

            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
        {
            var result = await basketRepository.SaveChangesAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(userName))
                await cache.RemoveAsync(userName!, cancellationToken);

            return result;
        }
    }
}
