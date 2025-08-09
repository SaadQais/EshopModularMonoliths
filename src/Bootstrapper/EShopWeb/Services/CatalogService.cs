namespace EShopWeb.Services
{
    public class CatalogService(ApiService apiService, ILogger<CatalogService> logger)
    {
        private readonly ApiService _apiService = apiService;
        private readonly ILogger<CatalogService> _logger = logger;

        public async Task<PaginatedResult<ProductDto>?> GetProductsAsync(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Getting products with pageIndex: {PageIndex}, pageSize: {PageSize}", pageIndex, pageSize);

                // Get the raw response as string
                var response = await _apiService.GetAsync<JsonElement>($"products/products?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.ValueKind != JsonValueKind.Undefined)
                {
                    // Check if the response has a "products" property
                    if (response.TryGetProperty("products", out var productsProperty))
                    {
                        return JsonSerializer.Deserialize<PaginatedResult<ProductDto>>(
                            productsProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                _logger.LogWarning("No products data received from API");
                return new PaginatedResult<ProductDto> { Data = [], Count = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return new PaginatedResult<ProductDto> { Data = [], Count = 0 };
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var response = await _apiService.GetAsync<JsonElement>($"/products/{productId}");

                if (response.ValueKind != JsonValueKind.Undefined)
                {
                    if (response.TryGetProperty("product", out var productProperty))
                    {
                        return JsonSerializer.Deserialize<ProductDto>(
                            productProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId}", productId);
                return null;
            }
        }

        public async Task<PaginatedResult<ProductDto>?> GetProductsByCategoryAsync(
            string category, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var response = await _apiService.GetAsync<JsonElement>(
                    $"/products/category?category={Uri.EscapeDataString(category)}&pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.ValueKind != JsonValueKind.Undefined)
                {
                    if (response.TryGetProperty("products", out var productsProperty))
                    {
                        return JsonSerializer.Deserialize<PaginatedResult<ProductDto>>(
                            productsProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                return new PaginatedResult<ProductDto> { Data = [], Count = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by category {Category}", category);
                return new PaginatedResult<ProductDto> { Data = [], Count = 0 };
            }
        }
    }
}
