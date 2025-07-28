namespace Catalog.Contracts.Products.DTOs
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Categories);
}
