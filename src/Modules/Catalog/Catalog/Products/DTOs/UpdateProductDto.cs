namespace Catalog.Products.DTOs
{
    public record UpdateProductDto(
        Guid Id,
        string Name, 
        string Description, 
        string ImageFile,
        decimal Price, 
        List<string> Categories);
}
