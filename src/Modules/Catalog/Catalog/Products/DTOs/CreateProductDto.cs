namespace Catalog.Products.DTOs
{
    public record CreateProductDto(
        string Name, 
        string Description, 
        string ImageFile,
        decimal Price, 
        List<string> Categories);
}
