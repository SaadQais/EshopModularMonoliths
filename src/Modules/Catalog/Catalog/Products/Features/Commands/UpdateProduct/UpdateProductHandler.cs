namespace Catalog.Products.Features.Commands.UpdateProduct
{
    public record UpdateProductCommand(UpdateProductDto Product) :
        ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    internal class UpdateProductHandler(CatalogDbContext context)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(
            UpdateProductCommand command, 
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FindAsync([command.Product.Id], cancellationToken) 
                    ?? throw new Exception($"Product not found: {command.Product.Id}");

            UpdateProduct(product, command.Product);

            context.Products.Update(product);
            await context.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }

        private static void UpdateProduct(Product product, UpdateProductDto productDto)
        {
            product.Update(
                productDto.Name, 
                productDto.Description, 
                productDto.ImageFile,
                productDto.Price, 
                productDto.Categories);
        }
    }
}
