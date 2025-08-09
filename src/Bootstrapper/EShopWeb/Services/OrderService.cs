namespace EShopWeb.Services
{
    public class OrderService(ApiService apiService, ILogger<OrderService> logger)
    {
        private readonly ApiService _apiService = apiService;
        private readonly ILogger<OrderService> _logger = logger;

        public async Task<PaginatedResult<OrderDto>?> GetOrdersAsync(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var response = await _apiService.GetAsync<object>($"/orders?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response != null)
                {
                    var jsonElement = (JsonElement)response;

                    if (jsonElement.TryGetProperty("orders", out var ordersProperty))
                    {
                        return JsonSerializer.Deserialize<PaginatedResult<OrderDto>>(
                            ordersProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                return new PaginatedResult<OrderDto> { Data = new List<OrderDto>(), Count = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                return new PaginatedResult<OrderDto> { Data = new List<OrderDto>(), Count = 0 };
            }
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var response = await _apiService.GetAsync<object>($"/orders/{orderId}");

                if (response != null)
                {
                    var jsonElement = (JsonElement)response;

                    if (jsonElement.TryGetProperty("order", out var orderProperty))
                    {
                        return JsonSerializer.Deserialize<OrderDto>(
                            orderProperty.GetRawText(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId}", orderId);
                return null;
            }
        }
    }
}
