namespace Catalog.Products.Features.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductDto Product) : 
        ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductHandler(CatalogDbContext context) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(
            CreateProductCommand command, 
            CancellationToken cancellationToken)
        {
            var product = CreateNewProduct(command.Product);

            await context.Products.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }

        private static Product CreateNewProduct(CreateProductDto product)
        {
            return Product.Create(
                product.Name, 
                product.Description, 
                product.ImageFile,
                product.Price, 
                product.Categories);
        }
    }
}
