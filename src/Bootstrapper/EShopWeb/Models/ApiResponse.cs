namespace EShopWeb.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = [];
    }

    public class PaginatedResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public List<T> Data { get; set; } = [];
    }
}
