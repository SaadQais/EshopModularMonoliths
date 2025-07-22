namespace Catalog.Products.Features.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductDto Product) : 
        ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Product.Categories).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

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
