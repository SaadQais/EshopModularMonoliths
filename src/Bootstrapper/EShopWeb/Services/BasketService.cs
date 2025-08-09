namespace EShopWeb.Services
{
    public class BasketService(ApiService apiService)
    {
        private readonly ApiService _apiService = apiService;

        public async Task<ShoppingCartDto?> GetBasketAsync(string userName)
        {
            var response = await _apiService.GetAsync<dynamic>($"/basket/{userName}");

            if (response?.shoppingCart != null)
            {
                return JsonSerializer.Deserialize<ShoppingCartDto>(
                    response.shoppingCart.ToString(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public async Task<bool> AddItemToBasketAsync(string userName, ShoppingCartItemDto item)
        {
            var request = new { UserName = userName, ShoppingCartItem = item };
            var response = await _apiService.PostAsync<object, dynamic>($"/basket/{userName}/item", request);
            return response != null;
        }

        public async Task<bool> RemoveItemFromBasketAsync(string userName, Guid productId)
        {
            return await _apiService.DeleteAsync($"/basket/{userName}/item/{productId}");
        }

        public async Task<bool> CreateBasketAsync(ShoppingCartDto basket)
        {
            var request = new { ShoppingCart = basket };
            var response = await _apiService.PostAsync<object, dynamic>("/basket", request);
            return response != null;
        }

        public async Task<bool> DeleteBasketAsync(string userName)
        {
            return await _apiService.DeleteAsync($"/basket/{userName}");
        }
    }
}
